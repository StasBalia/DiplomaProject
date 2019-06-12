using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SQLWorker.BLL;
using SQLWorker.BLL.ProvidersRepositories.Github;
using SQLWorker.BLL.ScriptUtilities;
using SQLWorker.DAL;
using SQLWorker.DAL.Repositories.Implementations;
using SQLWorker.DAL.Repositories.Interfaces;
using SQLWorker.Web.Models;

namespace SQLWorker.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<DatabaseSettings>(options => Configuration.GetSection("DatabaseSettings").Bind(options));
            services.Configure<GitHubCredentials>(
                options => Configuration.GetSection("GitHubCredentials").Bind(options));
            services.AddLogging(x => x.AddSerilog(dispose: true));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<IScriptRepository, PostgreSqlScriptRepository>(); //TODO: pass connstr to implementation
            services.AddTransient<IUserRepository, PostgreSqlUserRepository>();
            services.AddTransient<UserService>();
            services.AddTransient<ScriptWorker>();
            services.AddTransient<ScriptUpdater>();
            services.AddTransient<GithubPuller>();
            services.AddSingleton(_ => Configuration);
            //var t = Configuration.GetValue<string>("ProdDatabase");
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/LogIn";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                });
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .WriteTo.Console()
                .WriteTo.RollingFile("Logs\\{Date}_log.txt")
                .CreateLogger();
           
            var creds = Configuration.GetSection("GitHubCredentials").Get<GitHubCredentials>();
            GithubPuller p = new GithubPuller(creds);
            p.PullFromRepositories(Directory.GetDirectories(@"..\..\Repos\"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ScriptLoader s = new ScriptLoader();
            Task.Run(() => s.LoadScriptsAsync("Scripts/"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}