using System;
using Atlassian;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IBacklogProvider, BacklogProvider>()
                .Configure<AtlassianOptions>(configurationRoot.GetSection("Atlassian"))
                .BuildServiceProvider();

            var backlogProvider = serviceProvider.GetService<IBacklogProvider>();

            var items = backlogProvider.GetItemsAsync("Sprint Jan").Result;

            Console.WriteLine("Hello World!");
        }
    }
}
