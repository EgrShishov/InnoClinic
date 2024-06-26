
namespace Officies.Infrastructure.Persistence
{
    public class MongoDbSettings
    {
        public static string SectionName = "OfficesDatabase";
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string BooksCollectionName { get; set; } = null!;
    }
}
