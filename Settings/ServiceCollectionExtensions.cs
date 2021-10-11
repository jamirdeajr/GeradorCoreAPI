using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorCoreAPI.Settings
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritable<T>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json") where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var env1 = new HostingEnvironment
                {
                    EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                    ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                    ContentRootPath = AppDomain.CurrentDomain.BaseDirectory,
                    ContentRootFileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory)
                };

                var env2 = provider.GetService<IWebHostEnvironment>();

                
                var options = provider.GetService<IOptionsMonitor<T>>();

                if (env1 != null)
                    return new WritableOptions<T>(env1, options, section.Key, file);
                if (env2 != null)
                    return new WritableOptions<T>(env2, options, section.Key, file);

                return null;
            });
        }
    }
}
