using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Idionline.Models
{
    public class Idiom
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public char Index { get; set; }
        public string Pinyin { get; set; }
        public string Origin { get; set; }
        public List<Definition> Definitions { get; set; }
        public string Creator { get; set; }
        public long CreateTimeUT { get; set; }
        public string LastEditor { get; set; }
        public long UpdateTimeUT { get; set; }
    }
}
