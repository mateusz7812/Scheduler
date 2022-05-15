using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchedulerWebApplication.Models;

namespace SchedulerWebApplication.Mutations
{
    public class Mutation
    {
        public async Task<AccountOutput> CreateAccount(
            CreateAccountInput accountInput,
            [Service] SchedulerContext context
            )
        {
            var account = (await context.Accounts.AddAsync(new Account{Login = accountInput.Login, Password = accountInput.Password})).Entity;
            await context.SaveChangesAsync();
            return new AccountOutput(account.Id, account.Login);
        }
        
        public async Task<Executor> CreateExecutor(
            CreateExecutorInput executorInput,
            [Service] SchedulerContext context
        )
        {
            var executor = (await context.Executors.AddAsync(new Executor{Name = executorInput.Name, Description = executorInput.Description, AccountId = executorInput.AccountId})).Entity;
            await context.SaveChangesAsync();
            return await context.Executors.Include(t => t.Account).FirstAsync(t => t.Id == executor.Id);
        }
        
        public async Task<Models.Task> CreateTask(
            TaskInput taskInput,
            [Service] SchedulerContext context
        )
        {
            var vars = new Dictionary<string, string>();
            foreach (var variable in taskInput.DefaultEnvironmentVariables)
            {
                vars.Add(variable.Key, variable.Value);
            }
            var savedTask = (await context.Tasks.AddAsync( new Models.Task
            {
                Command = taskInput.Command,
                Name = taskInput.Name,
                InputType = taskInput.InputType,
                OutputType = taskInput.OutputType,
                DefaultEnvironmentVariables = vars
            })).Entity;
            await context.SaveChangesAsync();
            return savedTask;
        }
        
        public async Task<ExecutorStatus> CreateStatus(
            ExecutorStatusInput executorStatusInput,
            [Service] SchedulerContext context,
            [Service]ITopicEventSender eventSender
        )
        {
            var savedStatus = (await context.ExecutorStatuses.AddAsync( new ExecutorStatus
            {
                Date = executorStatusInput.Date,
                ExecutorId = executorStatusInput.ExecutorId,
                StatusCode = executorStatusInput.StatusCode
            })).Entity;
            await context.SaveChangesAsync();
            var accountId = context.Executors.Include(t => t.Account).First(t => t.Id == executorStatusInput.ExecutorId)
                .Account.Id;
            await eventSender
                .SendAsync($"account{accountId}" , savedStatus)
                .ConfigureAwait(false);
            return savedStatus;
        }

        public async Task<Flow> CreateFlowStart(
            int flowId,
            int executorId,
            [Service] SchedulerContext context,
            [Service] ITopicEventSender eventSender
        )
        {
            Flow flow = context.Flows.First(f => f.Id == flowId);
            await eventSender
                .SendAsync($"executor{executorId}", flow)
                .ConfigureAwait(false);
            return flow;
        }

        public async Task<List<FlowTask>> CreateFlowTasks(
            int flowTaskNumber,
            [Service] SchedulerContext context)
        {
            var randomTaskId = context.Tasks.First().Id;
            var flowTasks = Enumerable.Range(0, flowTaskNumber)
                .Select(_ => 
                    context.FlowTasks.Add(
                        new FlowTask()
                        {
                            TaskId = randomTaskId
                        }
                    )
                ).ToList();
            await context.SaveChangesAsync();
            return flowTasks.Select(e=> e.Entity).ToList();
        }

        public async Task<List<FlowTask>> UpdateFlowTasks(
            List<UpdateFlowTaskInput> flowTasks,
            [Service] SchedulerContext context)
        {
            foreach (var flowTask in flowTasks)
            {
                var updated = context.FlowTasks.Include(f=>f.Successors).First(f => f.Id == flowTask.Id);
                if (flowTask.TaskId is not null)
                    updated.TaskId = flowTask.TaskId.Value;
                if (flowTask.EnvironmentVariables is not null)
                {
                    var vars = new Dictionary<string, string>();
                    foreach (var variable in flowTask.EnvironmentVariables)
                    {
                        vars.Add(variable.Key, variable.Value);
                    }
                    updated.EnvironmentVariables = vars;
                }
                if(flowTask.SuccessorsIds is not null)
                    foreach (var successorId in flowTask.SuccessorsIds)
                    {
                        if(updated.SuccessorsIds.All(id => id != successorId))
                            context.StartingUps.Add(new StartingUp() {PredecessorId = updated.Id, SuccessorId = successorId});
                    }
            }
            await context.SaveChangesAsync();
            return flowTasks.Select(flowTask => context.FlowTasks.Include(f=>f.Successors).First(f => f.Id == flowTask.Id)).ToList();
        }
    }
}