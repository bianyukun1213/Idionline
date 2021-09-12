using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Idionline.Models
{
    public class Editor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SessionId { get; set; }
        public bool IsLimited { get; set; }
        public long RegistrationTimeUT { get; set; }
        public int EditCount { get; set; }
    }
}
