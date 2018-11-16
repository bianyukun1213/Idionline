using System.Collections.Generic;

namespace Idionline.Models
{
    public class Definition
    {
        public string Text { get; set; }
        public string Addition { get; set; }
        public bool IsMarked { get; set; }
        public Dictionary<string,string> Links { get; set; }
    }
}
