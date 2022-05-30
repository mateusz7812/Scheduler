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
        public async Task<AccountOutput> CreateLocalAccount(
            CreateLocalAccountInput accountInput,
            [Service] SchedulerContext context
            )
        {
            var account = (await context.LocalAccounts.AddAsync(new LocalAccount { Person = new Person { Login = accountInput.Login }, Password = accountInput.Password })).Entity;
            await context.SaveChangesAsync();
            return new AccountOutput(account.Person.Id, account.Person.Login);
        }

        public async Task<AccountOutput> CreateMicrosoftAccount(
            CreateMicrosoftAccountInput accountInput,
            [Service] SchedulerContext context
            )
        {
            var account = (await context.MicrosoftAccounts.AddAsync(new MicrosoftAccount { Person = new Person { Login = accountInput.Login }, MicrosoftAccountId = accountInput.MicrosoftAccountId })).Entity;
            await context.SaveChangesAsync();
            return new AccountOutput(account.Person.Id, account.Person.Login);
        }

        public async Task<Executor> CreateExecutor(
            CreateExecutorInput executorInput,
            [Service] SchedulerContext context
        )
        {
            var executor = (await context.Executors.AddAsync(new Executor{Name = executorInput.Name, Description = executorInput.Description, PersonId = executorInput.AccountId})).Entity;
            await context.SaveChangesAsync();
            return await context.Executors.Include(t => t.Person).FirstAsync(t => t.Id == executor.Id);
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

        public async Task<Flow> CreateFlow(
            FlowInput flowInput,
            [Service] SchedulerContext context
        )
        {
            var savedFlow = (await context.Flows.AddAsync(new Flow
            {
                Name = flowInput.Name,
                Description = flowInput.Description,
                PersonId = flowInput.AccountId
            })).Entity;
            await context.SaveChangesAsync();
            return savedFlow;
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
            var accountId = context.Executors.Include(t => t.Person).First(t => t.Id == executorStatusInput.ExecutorId)
                .Person.Id;
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
            context.FlowRuns.Add(new FlowRun
            {
                FlowId = flowId,
                ExecutorId = executorId,
                RunDate = DateTime.UtcNow.Ticks
            });
            await context.SaveChangesAsync();
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

        public async Task<Flow> UpdateFlow(
            UpdateFlowInput flow,
            [Service] SchedulerContext context)
        {
            var updated = context.Flows.First(f => f.Id == flow.Id);
            if(flow.Name is not null)
                updated.Name = flow.Name;
            if (flow.Description is not null)
                updated.Description = flow.Description;
            if (flow.FlowTaskId is not null)
                updated.FlowTaskId = flow.FlowTaskId;
            await context.SaveChangesAsync();
            return updated;
        }
    }
}