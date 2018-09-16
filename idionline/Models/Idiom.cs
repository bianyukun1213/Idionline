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
        public int ID { get; set; }
        //public List<Interpretation> Inter { get; set; }
        public string Interpretation { get; set; }
        public string Source { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdateDate { get; set; }
        public char Index { get; set; }
    }
}
