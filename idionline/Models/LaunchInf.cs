using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Idionline.Models
{
    public class LaunchInf
    {
        public string Text { get; set; }
        public string DailyIdiomName { get; set; }
        public int DailyIdiomId { get; set; }
        [Key]
        public long DateUT { get; set; }
    }
}
