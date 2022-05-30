using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SchedulerWebApplication.Models;

namespace SchedulerWebApplication
{
    public class SchedulerContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        public SchedulerContext(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<LocalAccount> LocalAccounts { get; set; }
        public DbSet<MicrosoftAccount> MicrosoftAccounts { get; set; }

        public DbSet<Executor> Executors { get; set; }
        
        public DbSet<ExecutorStatus> ExecutorStatuses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Flow> Flows { get; set; }
        public DbSet<FlowRun> FlowRuns { get; set; }
        public DbSet<FlowTask> FlowTasks { get; set; }
        public DbSet<StartingUp> StartingUps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=scheduler.db");
            options.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlowTask>()
                .Property(b => b.EnvironmentVariables)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
            
            modelBuilder.Entity<Task>()
                .Property(b => b.DefaultEnvironmentVariables)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));

            modelBuilder.Entity<Person>()
                .HasMany(t => t.Executors)
                .WithOne(t => t.Person)
                .HasForeignKey(t => t.PersonId);

            modelBuilder.Entity<Person>()
                .HasMany<LocalAccount>()
                .WithOne(t => t.Person)
                .HasForeignKey(t => t.PersonId);

            modelBuilder.Entity<Person>()
                .HasMany<MicrosoftAccount>()
                .WithOne(t => t.Person)
                .HasForeignKey(t => t.PersonId);

            modelBuilder.Entity<Executor>()
                .HasMany(t => t.Statuses)
                .WithOne()
                .HasForeignKey(t => t.ExecutorId);
            
            modelBuilder.Entity<Person>()
                .HasMany(t => t.Flows)
                .WithOne(t => t.Person)
                .HasForeignKey(t => t.PersonId);

            modelBuilder.Entity<Task>()
                .HasMany(t => t.FlowTasks)
                .WithOne(t => t.Task)
                .HasForeignKey(t => t.TaskId);

            modelBuilder.Entity<FlowTask>()
                .HasOne<Flow>(t => t.Flow)
                .WithOne(t => t.FlowTask)
                .HasForeignKey<Flow>(t => t.FlowTaskId);
            
            modelBuilder.Entity<StartingUp>()
                .HasKey(p => new {p.PredecessorId, p.SuccessorId});

            modelBuilder.Entity<StartingUp>()
                .HasOne(t => t.Predecessor)
                .WithMany(t => t.Successors)
                .HasForeignKey(t => t.PredecessorId);

            modelBuilder.Entity<StartingUp>()
                .HasOne(t => t.Successor)
                .WithMany(t => t.Predecessors)
                .HasForeignKey(t => t.SuccessorId);

            modelBuilder.Entity<FlowRun>()
                .HasOne(f => f.Executor)
                .WithMany(e => e.FlowRuns)
                .HasForeignKey(f => f.ExecutorId);

            modelBuilder.Entity<FlowRun>()
                .HasOne(f => f.Flow)
                .WithMany(e => e.FlowRuns)
                .HasForeignKey(f => f.FlowId);

            /*modelBuilder.Entity<FlowTask>()
                .HasMany<FlowTask>(t => t.Predecessors)
                .WithMany(t => t.Successors)
                .UsingEntity<StartingUp>(
                    typeof(StartingUp), 
                    t=> t.HasOne<FlowTask>(t => t.Predecessor).WithMany(t => t.Predecessors).HasForeignKey(t => t.PredecessorId),
                    t=> t.HasOne<FlowTask>(t => t.Predecessor).WithMany(t => t.Predecessors).HasForeignKey(t => t.PredecessorId),
                    );
            */
        }
    }
}