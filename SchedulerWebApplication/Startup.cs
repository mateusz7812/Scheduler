using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;
using SchedulerWebApplication.Models;
using SchedulerWebApplication.Mutations;

namespace SchedulerWebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRouting()
                .AddDbContext<SchedulerContext>()
                .AddGraphQLServer()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseGraphQL();
            //app.UsePlayground();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGraphQL();
                endpoints.MapGraphQLPlayground();
            });
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SchedulerContext>();
                if (context.Database.EnsureCreated())
                {
                    var account = new Account { Login = "testLogin", Password = "testPassword"};
                    
                    context.Executors.Add(new Executor
                    {
                        Account = account,
                        Name = "testName",
                        Description = "testDescription"
                    });
                    
                    context.Executors.Add(new Executor
                    {
                        Account = account,
                        Name = "testName2",
                        Description = "testDescription2"
                    });
                    
                    context.Accounts.Add(new Account
                    {
                        Login = "testLogin2",
                        Password = "testPassword2"
                    });

                    var task1 = context.Tasks.Add(new Task
                    {
                        InputType = typeof(string).ToString(),
                        OutputType = typeof(void).ToString(),
                        Name = "Print",
                        Command = "Write-Output"
                    });
                        
                    var task2 = context.Tasks.Add(new Task
                    {
                        InputType = typeof(string).ToString(),
                        OutputType = typeof(void).ToString(),
                        Name = "Print2",
                        Command = "Write-Output"
                    });

                    context.SaveChanges();
                    var flowTask1 = context.FlowTasks.Add(new FlowTask
                    {
                        TaskId = task1.Entity.Id
                    });
                    
                    var flowTask2 = context.FlowTasks.Add(new FlowTask
                    {
                        TaskId = task2.Entity.Id
                    });

                    context.SaveChanges();
                    context.StartingUps.Add(new StartingUp
                    {
                        PredecessorId = flowTask2.Entity.Id,
                        SuccessorId = flowTask1.Entity.Id
                    });
                    
                    context.Flows.Add(new Flow
                    {
                        AccountId = 2,
                        Description = "testDescription",
                        Name = "testName",
                        FlowTaskId = flowTask1.Entity.Id
                    });
                    
                    context.SaveChangesAsync();
                }
            }
        }
    }
}