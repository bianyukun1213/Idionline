using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idionline.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.AspNetCore.TimedJob;

namespace Idionline.Jobs
{
    public class AddDailyIdiom : Job
    {
        [Invoke(Begin = "2018-09-25 00:00", Interval = 1000 * 3600 * 24, SkipWhileExecuting = true)]
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
