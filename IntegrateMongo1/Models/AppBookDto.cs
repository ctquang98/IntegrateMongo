using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IntegrateMongo1.Models
{
    public class AppBookDto
    {
        public string BookName { get; set; }
        public int Price { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
    }
}
