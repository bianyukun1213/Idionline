using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Idionline.Models
{
    public class EditorLoginInfo
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string SessionId { get; set; }
    }
}
