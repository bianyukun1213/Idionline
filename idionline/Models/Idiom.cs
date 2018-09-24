using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Idionline.Models
{
    public class Idiom
    {
        public string IdiomName { get; set; }
        public int Id { get; set; }
        public string Interpretation { get; set; }
        public string Source { get; set; }
        public string LastEditor { get; set; }
        public long UpdateTimeUT { get; set; }
        public char Index { get; set; }
    }
}
