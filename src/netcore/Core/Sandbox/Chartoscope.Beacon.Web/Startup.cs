using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Chartoscope.Core.Messaging;
using Chartoscope.Beacon.Shell;

namespace Chartoscope.Beacon.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            BeaconAdminClient _adminClient = new BeaconAdminClient("127.0.0.1", 5555);

            if (!_adminClient.IsListening())
            {
                string basePath = PlatformServices.Default.Application.ApplicationBasePath;
                string commandLine = Path.Combine(basePath, "Chartoscope.Beacon.Shell.dll PUB 12345 5555");
                var startInfo = new ProcessStartInfo("dotnet", commandLine);
                Process.Start(startInfo);
            }
                        
            var beaconServiceStatus= _adminClient.GetStatus();

            if (beaconServiceStatus==BeaconServiceStatusOption.Offline)
            {
                _adminClient.TakeOnline();
            }
            
         }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            // Add SignalR
            services.AddSignalR(options =>
            {                
                options.Hubs.EnableJavaScriptProxies = true;
                options.Hubs.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseWebSockets();
            app.UseSignalR();            
        }
    }
}
