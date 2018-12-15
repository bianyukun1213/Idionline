using System.Collections.Generic;

namespace Idionline.Models
{
    public class Definition
    {
        public string Source { get; set; }
        public string Text { get; set; }
        public string Addition { get; set; }
        public bool IsBold { get; set; }
        public Dictionary<string, string> Links { get; set; }
    }
}