using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AviDev.FileStream.StressTest
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Starting program");
            var services =
                new ServiceCollection()
                    .AddSingleton<Stress>();

            services
                .AddHttpClient("FileStream", x => x.BaseAddress = new Uri("http://localhost:50045/api/File/"));

            var serviceProvider =
                services
                    .BuildServiceProvider();

            await CallStressTests(serviceProvider);
        }

        static async Task CallStressTests(IServiceProvider serviceProvider)
        {
            var tasks = new List<Task<int>>();

            Func<Task<int>> obterTaskStress = 
                () => serviceProvider
                        .GetService<Stress>()
                        .StressImageApi();

            for (int i = 0; i < 10; i++)
                tasks.Add(obterTaskStress());

            while (true)
            {
                var task = await Task.WhenAny(tasks);
                tasks.Remove(task);
                tasks.Add(obterTaskStress());
                Console.WriteLine($"Image {task.Result} successfully processed");

            }
        }
    }
}