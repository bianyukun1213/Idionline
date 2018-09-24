using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Idionline.Models;
using Hangfire;
using Hangfire.SQLite;

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
            services.AddDbContext<IdionlineContext>(options => options.UseSqlite("Data Source=Idionline.db;"));
            services.AddHangfire(options => options.UseSQLiteStorage("Data Source=Idionline.db;"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
            app.UseHangfireServer();
            RecurringJob.AddOrUpdate(() => AddIdiom2Db(), Cron.Daily);
        }
        public static void AddIdiom2Db()
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdionlineContext>();
            optionsBuilder.UseSqlite("Data Source=Idionline.db;");
            IdionlineContext context = new IdionlineContext(optionsBuilder.Options);
            var items = from m in context.Idioms select m.IdiomName;
            List<string> ls = items.ToList<string>();
            Random r = new Random();
            int i = r.Next(ls.Count);
            DateTimeOffset dateUT = DateTimeOffset.Now;
            int hour = dateUT.Hour;
            int min = dateUT.Minute;
            int sec = dateUT.Second;
            dateUT = dateUT.AddSeconds(-sec).AddMinutes(-min).AddHours(-hour);
            context.LaunchInfs.Add(new LaunchInf { Text = null, DailyIdiom = ls[i], DateUT = dateUT.ToUnixTimeSeconds() });
            context.SaveChanges();
        }
    }
}
