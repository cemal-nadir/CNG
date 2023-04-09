#nullable enable
using CNG.MongoDB.Configuration;
using Humanizer;

namespace CNG.MongoDB.Extensions
{
  public static class MongoDbConfigurationExtensions
  {
    public static string GetCollectionName<TEntity>(this MongoDbRepositoryOptions options)
    {
      var str = typeof (TEntity).Name;
      if (options.PluralizeCollectionNames)
        str = str.Pluralize();
      var namingConvention = options.CollectionNamingConvention;
      if (true)
      {
      }

      string collectionName;
      switch (namingConvention)
      {
        case NamingConvention.LowerCase:
          collectionName = str.ToLower();
          break;
        case NamingConvention.UpperCase:
          collectionName = str.ToUpper();
          break;
        case NamingConvention.Pascal:
          collectionName = InflectorExtensions.Pascalize(str);
          break;
        case NamingConvention.Camel:
          collectionName = InflectorExtensions.Camelize(str);
          break;
        case NamingConvention.Snake:
          collectionName = InflectorExtensions.Underscore(str);
          break;
        default:
          throw new ArgumentOutOfRangeException("options", options.CollectionNamingConvention, "Unknown collection naming convention");
      }
      if (true)
      {
      }

      return collectionName;
    }
  }
}
