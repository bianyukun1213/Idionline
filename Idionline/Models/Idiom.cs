using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Idionline.Models
{
    public class Idiom
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public char Index { get; set; }
        public string Pinyin { get; set; }
        public string Origin { get; set; }
        public List<string> Tags { get; set; }
        public List<Definition> Definitions { get; set; }
        public string Creator { get; set; }
        public long CreationTimeUT { get; set; }
        public string LastEditor { get; set; }
        public long UpdateTimeUT { get; set; }
        public bool ToBeCorrected { get; set; }
    }
}
