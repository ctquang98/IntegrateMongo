using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IntegrateMongo1.Models
{
    public class AppCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
