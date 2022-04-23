using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
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
            var savedTask = (await context.Tasks.AddAsync( new Models.Task
            {
                Command = taskInput.Command,
                Name = taskInput.Name,
                InputType = taskInput.InputType,
                OutputType = taskInput.OutputType
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
    }
}