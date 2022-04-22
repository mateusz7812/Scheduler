using System.Threading.Tasks;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using SchedulerWebApplication.Models;

namespace SchedulerWebApplication.Mutations
{
    public class Mutation
    {
        public async Task<AccountOutput> CreateAccount(
            AccountInput accountInput,
            [Service] SchedulerContext context
            )
        {
            var account = (await context.Accounts.AddAsync(new Account{Login = accountInput.Login, Password = accountInput.Password})).Entity;
            await context.SaveChangesAsync();
            return new AccountOutput(account.Id, account.Login);
        }
        
        public async Task<Executor> CreateExecutor(
            ExecutorInput executorInput,
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
    }
}