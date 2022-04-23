using System.Linq;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using SchedulerWebApplication.Models;

namespace SchedulerWebApplication
{
    public class Query
    {
        private IQueryable<Account> accounts;

        public IQueryable<Account> GetAccounts([Service]SchedulerContext context) =>
            context.Accounts.Include(b => b.Executors).Include(a => a.Flows);
        
        public Account GetLogin([Service]SchedulerContext context, string login, string password)
        {
            accounts = context.Accounts.Include(b => b.Executors).Include(a => a.Flows)
                .Where(p => p.Login.Equals(login) && p.Password.Equals(password));
            if(accounts.Any())
                return accounts.First();
            return null;
        }

        public IQueryable<Executor> GetExecutorsForAccount([Service]SchedulerContext context, int accountId) =>
            context.Executors.Where(t => t.AccountId == accountId);//.Include(b => b.Executors);
        
        public IQueryable<Flow> GetFlowsForAccount([Service]SchedulerContext context, int accountId) =>
            context.Flows.Where(t => t.AccountId == accountId);//.Include(b => b.Executors);

        public IQueryable<Task> GetTasks([Service]SchedulerContext context) =>
            context.Tasks;

    }
}