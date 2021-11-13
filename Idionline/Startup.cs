using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Mongo;
using Idionline.Models;
using Microsoft.Extensions.Hosting;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;

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
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy(),
            };
            var storageOptions = new MongoStorageOptions
            {
                MigrationOptions = migrationOptions,
                CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
            };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("zh-CN");
            });
            services.AddLocalization();
            services.Configure<IdionlineSettings>(Configuration.GetSection("IdionlineSettings"));
            services.AddHangfire(options => options.UseMongoStorage("mongodb://localhost/db_idionline", storageOptions));
            services.AddControllers();
            services.AddHttpClient();
            services.AddTransient<DataAccess>();
            services.AddHangfireServer(options => { options.WorkerCount = 1; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var support = new List<CultureInfo>()
            {
                new CultureInfo("zh-CN"),
                new CultureInfo("zh-HK"),
                new CultureInfo("zh-TW")
            };
            app.UseRequestLocalization(x =>
            {
                x.SetDefaultCulture("zh-CN");
                x.SupportedCultures = support;
                x.SupportedUICultures = support;
                x.AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider());
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            app.UseHttpsRedirection();
            app.UseHangfireDashboard();
            //生成每日成语。
            RecurringJob.AddOrUpdate<DataAccess>(x => x.GenLI(), Cron.Daily, TimeZoneInfo.Local);
        }
    }
}
