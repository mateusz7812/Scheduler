using System;
using System.Collections.Generic;
using System.Linq;
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
using SchedulerWebApplication.Subscriptions;

namespace SchedulerWebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors(options =>
                    options.AddDefaultPolicy(policy =>
                        policy.WithOrigins("*").AllowAnyHeader()
                    )
                )
                .AddRouting()
                .AddDbContext<SchedulerContext>()
                .AddGraphQLServer()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .AddSubscriptionType<Subscription>()
                .AddInMemorySubscriptions()
                .AddApolloTracing();
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
            app.UseCors();
            app.UseWebSockets();
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
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SchedulerContext>();
                if (context.Database.EnsureCreated())
                {
                    var account = new Person { Login = "testLogin"};

                    context.LocalAccounts.Add(new LocalAccount 
                    { 
                        Person = account, 
                        Password = "testPassword" 
                    });

                    context.MicrosoftAccounts.Add(new MicrosoftAccount
                    {
                        Person = account,
                        MicrosoftAccountId = Guid.Parse("b2c4e305-63ed-431b-baef-bb051f70d749")
                    });
                    
                    context.Executors.Add(new Executor
                    {
                        Person = account,
                        Name = "testName",
                        Description = "testDescription"
                    });
                    
                    context.Executors.Add(new Executor
                    {
                        Person = account,
                        Name = "testName2",
                        Description = "testDescription2"
                    });
                    
                    var person = new Person
                    {
                        Login = "testLogin2",
                    };

                    context.LocalAccounts.Add(new LocalAccount
                    {
                        Person = person,
                        Password = "testPassword2"
                    });

                    var task1 = context.Tasks.Add(new Task
                    {
                        InputType = typeof(string).ToString(),
                        OutputType = typeof(void).ToString(),
                        Name = "PrintText",
                        Command = "Write-Output $Env:text",
                        DefaultEnvironmentVariables = new Dictionary<string, string>(){{"text", "Hello world!"}}
                    });
                        
                    var task2 = context.Tasks.Add(new Task
                    {
                        InputType = typeof(string).ToString(),
                        OutputType = typeof(void).ToString(),
                        Name = "PrintSum",
                        Command = "[int]$Env:a + [int]$Env:b | Write-Output ",
                        DefaultEnvironmentVariables = new Dictionary<string, string>(){{"a", "1"}, {"b", "2"}}
                    });

                    context.SaveChanges();
                    var flowTask1 = context.FlowTasks.Add(new FlowTask
                    {
                        TaskId = task1.Entity.Id,
                        EnvironmentVariables = new Dictionary<string, string>(){{"text", "test1"}}
                    });
                    
                    var flowTask2 = context.FlowTasks.Add(new FlowTask
                    {
                        TaskId = task2.Entity.Id,
                        EnvironmentVariables = new Dictionary<string, string>(){{"a", "4"}, {"b", "15"}}
                    });

                    var flowTask3 = context.FlowTasks.Add(new FlowTask
                    {
                        TaskId = task1.Entity.Id,
                        EnvironmentVariables = new Dictionary<string, string>(){{"text", "Hello test!"}}
                    });
                    
                    context.ExecutorStatuses.Add(new ExecutorStatus{Date = 123, StatusCode = ExecutorStatusCode.Online, ExecutorId = 1});
                    context.ExecutorStatuses.Add(new ExecutorStatus{Date = 121, StatusCode = ExecutorStatusCode.Offline, ExecutorId = 1});
                    context.ExecutorStatuses.Add(new ExecutorStatus{Date = 124, StatusCode = ExecutorStatusCode.Working, ExecutorId = 1});

                    context.SaveChanges();
                    context.StartingUps.Add(new StartingUp
                    {
                        PredecessorId = 1,
                        SuccessorId = 2
                    });
                    
                    context.StartingUps.Add(new StartingUp
                    {
                        PredecessorId = 2,
                        SuccessorId = 3
                    });
                    
                    context.Flows.Add(new Flow
                    {
                        PersonId = 1,
                        Description = "testDescription",
                        Name = "testName",
                        FlowTaskId = 1
                    });
                    
                    context.SaveChangesAsync();

                    int flowId = context.Flows.First().Id;
                    int executorId = context.Executors.First().Id;
                    context.FlowRuns.Add(new FlowRun
                    {
                        RunDate = new DateTime(2012, 10, 20, 10, 15, 21).Ticks,
                        FlowId = flowId,
                        ExecutorId = executorId
                    });
                    context.FlowRuns.Add(new FlowRun
                    {
                        RunDate = new DateTime(2012, 10, 20, 11, 15, 21).Ticks,
                        FlowId = flowId,
                        ExecutorId = executorId
                    });
                    context.FlowRuns.Add(new FlowRun
                    {
                        RunDate = new DateTime(2012, 10, 20, 11, 30, 21).Ticks,
                        FlowId = flowId,
                        ExecutorId = executorId
                    });

                    context.SaveChangesAsync();
                }
            }
        }
    }
}