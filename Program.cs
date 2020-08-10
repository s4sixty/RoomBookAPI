using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RoomBookAPI.Models;
using RoomBookAPI.Helpers;

namespace RoomBookAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Get the Host which will host this application.
            var host = CreateHostBuilder(args).Build();

            // Find the service layer within our scope.
            using (var scope = host.Services.CreateScope())
            {
                // Get the instance of ApiContext in our services layer
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApiContext>();

                // Call the DataGenerator to create sample data
                DataGenerator.Initialize(services);
            }

            //Continue to run the application
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
