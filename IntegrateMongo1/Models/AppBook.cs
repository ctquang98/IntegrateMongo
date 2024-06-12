using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IntegrateMongo1.Models
{
    public class AppBook
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string BookName { get; set; }
        [BsonElement("price")]
        public int Price { get; set; }
        [BsonElement("author")]
        public string Author { get; set; }
        [BsonElement("category")]
        public string Category { get; set; }
    }
}
