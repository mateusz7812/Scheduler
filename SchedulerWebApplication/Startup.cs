using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GraphQL.StarWars;

namespace SchedulerWebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            GraphQL.MicrosoftDI.GraphQLBuilderExtensions.AddGraphQL(services, builder =>
                builder.AddServer(true, options => options.EnableMetrics = true)
                    .AddUserContextBuilder(httpContext => new GraphQLUserContext {User = httpContext.User})
                    .AddSystemTextJson()
                    .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                    .AddSchema<StarWarsSchema>()
                    .AddGraphTypes(typeof(StarWarsSchema).Assembly)
            );

            services.AddSingleton<StarWarsData>();
            services.AddLogging(builder => builder.AddConsole());
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground();
            /*app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });*/
        }
    }
}