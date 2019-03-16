using System.Collections.Generic;

namespace Idionline.Models
{
    public class UpdateData
    {
        public string OpenId { get; set; }
        public bool BsonMode { get; set; }
        public string BsonStr { get; set; }
        public List<DefinitionUpdate> Updates { get; set; }
    }
}