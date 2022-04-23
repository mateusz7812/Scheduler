using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Executor> Executors { get; set; }
        
        public DbSet<ExecutorStatus> ExecutorStatuses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Flow> Flows { get; set; }
        public DbSet<FlowTask> FlowTasks { get; set; }
        public DbSet<StartingUp> StartingUps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=scheduler.db");
            options.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(t => t.Executors)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Executor>()
                .HasMany(t => t.Statuses)
                .WithOne()
                .HasForeignKey(t => t.ExecutorId);
            
            modelBuilder.Entity<Account>()
                .HasMany(t => t.Flows)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);

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
                .WithMany(t => t.Predecessors)
                .HasForeignKey(t => t.PredecessorId);

            modelBuilder.Entity<StartingUp>()
                .HasOne(t => t.Successor)
                .WithMany(t => t.Successors)
                .HasForeignKey(t => t.SuccessorId);
            
            
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