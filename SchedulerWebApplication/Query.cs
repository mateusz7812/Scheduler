using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using SchedulerWebApplication.Models;

namespace SchedulerWebApplication
{
    public class Query
    {
        public IQueryable<Person> GetAccounts(
            [Service] SchedulerContext context ) =>
            context.Persons.Include(b => b.Executors).ThenInclude(t => t.Statuses).Include(a => a.Flows);
        
        public Person GetLocalLogin(
            [Service] SchedulerContext context, 
            string login, 
            string password)
        {
            var accounts = context.LocalAccounts.Include(p => p.Person)
                .Where(p => p.Person.Login.Equals(login) && p.Password.Equals(password));
            if(accounts.Any())
                return accounts.First().Person;
            return null;
        }

        public Person GetMicrosoftLogin(
            [Service] SchedulerContext context,
            string microsoftAccountId)
        {
            var accounts = context.MicrosoftAccounts.Include(p => p.Person)
                .Where(p => p.MicrosoftAccountId == Guid.Parse(microsoftAccountId));
            if (accounts.Any())
                return accounts.First().Person;
            return null;
        }

        public IQueryable<Executor> GetExecutorsForAccount(
            [Service] SchedulerContext context, 
            int accountId) =>
            context.Executors.Where(t => t.PersonId == accountId).Include(b => b.Statuses);
        
        public IQueryable<Flow> GetFlowsForAccount(
            [Service] SchedulerContext context, 
            int accountId) =>
            context.Flows.Where(t => t.PersonId == accountId);//.Include(b => b.Executors);

        public IQueryable<Task> GetTasks([Service]SchedulerContext context) =>
            context.Tasks;

        public IQueryable<FlowRun> GetFlowRunsForExecutor(
            [Service] SchedulerContext context,
            int executorId) =>
                context.FlowRuns.Where(f => f.ExecutorId == executorId);

        public IQueryable<FlowRun> GetFlowRunsForFlow(
            [Service] SchedulerContext context,
            int flowId) =>
                context.FlowRuns.Where(f => f.FlowId == flowId);

        public IQueryable<FlowTask> GetFlowTasksForFlow(
            [Service]SchedulerContext context, 
            int flowId)
        {
            List<FlowTask> flowTasks = new List<FlowTask>();
            var flow = context.Flows.First(t => t.Id == flowId);
            if (flow.FlowTaskId is not null)
            {
                int i = 0;
                flowTasks.Add(context.FlowTasks.Include(t => t.Successors).Include(t => t.Task).First(t => t.Id == flow.FlowTaskId));
                do
                {
                    flowTasks.AddRange(flowTasks[i].Successors
                        .Select(t => t.SuccessorId)
                        .Where(id => !flowTasks.Any(f => f.Id == id))
                        .Select(id =>
                            context.FlowTasks.Include(t => t.Successors).Include(t => t.Task).First(t => t.Id == id)
                        ));
                    i++;
                } while (i < flowTasks.Count);

            }
            return flowTasks.AsQueryable();
        }
    }
}