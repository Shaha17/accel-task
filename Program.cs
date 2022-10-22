using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LinkShortener
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                IronBarCode.License.LicenseKey = "IRONBARCODE.GEVEDA1831.25743-36F85E31A3-QIAJL5A6I3YLGLKO-CWQOFB5BYTKO-6NARTBTHOGBI-PJ4AE7LAPY3E-IHN7KQYPBKZQ-CDM7JN-TMIRVHQKMPOIEA-DEPLOYMENT.TRIAL-AFML5H.TRIAL.EXPIRES.20.NOV.2022";
                var context = services.GetRequiredService<AppDbContext>();
                var webHostEnvironment = services.GetRequiredService<IWebHostEnvironment>();
                
                if (!Directory.Exists(webHostEnvironment.WebRootPath))
                {
                    var dir = Directory.CreateDirectory(webHostEnvironment.WebRootPath);
                    logger.LogInformation("Folder for static files created at: " + dir.FullName);
                }

                if (!Directory.Exists(Path.Combine(webHostEnvironment.WebRootPath, "qr")))
                {
                    var dir = Directory.CreateDirectory(Path.Combine(webHostEnvironment.WebRootPath, "qr"));
                    logger.LogInformation("Folder for images created at: " + dir.FullName);
                }

                await context.Database.MigrateAsync();
                logger.LogInformation("Migrate successful");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}