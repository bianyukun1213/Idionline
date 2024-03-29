using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Idionline.Models
{
    public class LaunchInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> ArgsDic { get; set; }
        public string Text { get; set; }
        public string ThemeColor { get; set; }
        public string LogoUrl { get; set; }
        public Idiom DailyIdiom { get; set; }
        public long IdiomsCount { get; set; }
        public long DateUT { get; set; }
    }
}
