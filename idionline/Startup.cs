using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Mongo;
using Idionline.Models;

namespace Idionline
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationOptions = new MongoMigrationOptions
            {
                Strategy = MongoMigrationStrategy.Migrate,
                BackupStrategy = MongoBackupStrategy.Collections
            };
            var storageOptions = new MongoStorageOptions
            {
                MigrationOptions = migrationOptions
            };
            services.Configure<IdionlineSettings>(this.Configuration.GetSection("IdionlineSettings"));
            services.AddHangfire(options => options.UseMongoStorage("mongodb://localhost", "IdionlineDB", storageOptions));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddHttpClient();
            services.AddTransient<DataAccess>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseHangfireDashboard();
            var options = new BackgroundJobServerOptions { WorkerCount = 1 };
            app.UseHangfireServer(options);
            //生成每日成语。
            RecurringJob.AddOrUpdate<DataAccess>(x => x.GenLI(), Cron.Daily, TimeZoneInfo.Local);
        }
    }
}
