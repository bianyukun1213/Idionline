using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Idionline.Models
{
    public class Editor
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
    }
}
