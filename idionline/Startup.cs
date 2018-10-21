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
            var options = new BackgroundJobServerOptions { WorkerCount = 1 };
            app.UseHangfireServer(options);
            RecurringJob.AddOrUpdate(() => AddIdiom2Db(), Cron.Daily);
        }
        public static void AddIdiom2Db()
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdionlineContext>();
            optionsBuilder.UseSqlite("Data Source=Idionline.db;");
            IdionlineContext context = new IdionlineContext(optionsBuilder.Options);
            DateTimeOffset dateUT = DateTimeOffset.Now;
            int hour = dateUT.Hour;
            int min = dateUT.Minute;
            int sec = dateUT.Second;
            long dateL = dateUT.AddSeconds(-sec).AddMinutes(-min).AddHours(-hour).ToUnixTimeSeconds();
            var item = context.LaunchInfs.Find(dateL);
            var items = from m in context.Idioms select m.Id;
            List<int> ls = items.ToList<int>();
            Random r = new Random();
            int i = r.Next(ls.Count);
            if (item == null)
            {
                context.LaunchInfs.Add(new LaunchInf { Text = null, DailyIdiomName = context.Idioms.Find(ls[i]).IdiomName, DailyIdiomId = ls[i], DateUT = dateL });
            }
            else
            {
                if (item.DailyIdiomName == null)
                {
                    string text = item.Text;
                    context.Remove(item);
                    context.LaunchInfs.Add(new LaunchInf { Text = text, DailyIdiomName = context.Idioms.Find(ls[i]).IdiomName, DailyIdiomId = ls[i], DateUT = dateL });
                }
            }
            context.SaveChanges();
        }
    }
}
