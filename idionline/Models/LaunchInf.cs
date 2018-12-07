using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Idionline.Models
{
    public class LaunchInf
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public string MainColor { get; set; }
        public string LogoUrl { get; set; }
        public bool DisableAds { get; set; }
        //public Dictionary<string,string> FloatEasterEggs { get; set; }
        public string DailyIdiomId { get; set; }
        public long DateUT { get; set; }
    }
}