using System.Collections.Generic;

namespace Idionline.Models
{
    public class UpdateData
    {
        //public string SessionId { get; set; }
        public string BsonStr { get; set; }
        public string Name { get; set; }
        public char Index { get; set; }
        public string Pinyin { get; set; }
        public string Origin { get; set; }
        public List<DefinitionUpdate> DefinitionUpdates { get; set; }
        public bool ToBeCorrected { get; set; }

    }
}