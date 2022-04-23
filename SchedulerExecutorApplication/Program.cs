using System;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SchedulerExecutorApplication.GraphQl;
using StrawberryShake;

namespace SchedulerExecutorApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSchedulerServer()
                .ConfigureHttpClient(client =>
                    client.BaseAddress = new Uri("http://localhost:3000/graphql"));
            IServiceProvider services = serviceCollection.BuildServiceProvider();
            ISchedulerServer server = services.GetRequiredService<ISchedulerServer>();

            await ConfigureIfNeeded(server);
            Console.WriteLine("");
        }

        private static async Task ConfigureIfNeeded(ISchedulerServer schedulerServer)
        {
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent;
            var configPath = Path.Combine(currentDirectory!.FullName, "app.config");
            var executablePath = Path.Combine(currentDirectory!.FullName, "Program.cs");
            /*if (!File.Exists(configPath))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sb.AppendLine("<configuration>");
                sb.AppendLine("</configuration>");
                File.WriteAllText(configPath, sb.ToString());
                Console.WriteLine("Config file created");
            }*/

            Configuration config = ConfigurationManager.OpenExeConfiguration(executablePath);

            if (!config.AppSettings.Settings.AllKeys.Contains("accountId"))
            {
                Console.WriteLine("Account not found, before running flows you have to login");
                IOperationResult<IGetLoginResult> result = null;
                while (true)
                {
                    Console.Write("Login: ");

                    string login = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = ReadPassword();
                    Console.WriteLine("Logging in...");

                    result = await schedulerServer.GetLogin.ExecuteAsync(login, password);
                    result.EnsureNoErrors();
                    if (result?.Data?.Login is not null)
                    {
                        break;
                    }

                    Console.WriteLine("User not found");
                }

                config.AppSettings.Settings.Add("accountId", result.Data.Login.Id.ToString());
                config.AppSettings.Settings.Add("accountLogin", result.Data.Login.Login);
                config.Save(ConfigurationSaveMode.Minimal);
            }
            
            Console.WriteLine($"User {config.AppSettings.Settings["accountLogin"].Value} with id {config.AppSettings.Settings["accountId"].Value} logged in");

            if (!config.AppSettings.Settings.AllKeys.Contains("executorId"))
            {
                Console.WriteLine("Executor not registered");
                Console.Write("Executor name: ");
                var name = Console.ReadLine();
                Console.WriteLine("Executor description: ");
                var description = Console.ReadLine();
                var accountId = Int32.Parse(config.AppSettings.Settings["accountId"].Value);
                var executorInput = new ExecutorInput { AccountId = accountId, Name = name, Description = description};
                var result = await schedulerServer.CreateExecutor.ExecuteAsync(executorInput);
                result.EnsureNoErrors();
                config.AppSettings.Settings.Add("executorId", result.Data?.CreateExecutor?.Id.ToString());
                config.Save(ConfigurationSaveMode.Minimal);
            }
            Console.WriteLine($"Executor is registered with id {config.AppSettings.Settings["executorId"].Value}");

        }

        private static string ReadPassword()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();

            return pass;
        }
    }
}