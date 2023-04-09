#nullable enable
namespace CNG.MongoDB.Configuration
{
  public class MongoDbRepositoryOptions
  {
    public string ConnectionString { get; set; } = string.Empty;

    public string DbName { get; set; } = string.Empty;

    public NamingConvention CollectionNamingConvention { get; set; } = NamingConvention.Snake;

    public bool PluralizeCollectionNames { get; set; } = true;
  }
}
