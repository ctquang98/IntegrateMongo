namespace IntegrateMongo1.Models
{
    public class AppDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string BooksCollectionName { get; set; } = "books";
    }
}
