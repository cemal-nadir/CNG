#nullable enable
using System.Reflection;
using CNG.Abstractions.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CNG.MongoDB.Configuration
{
  public class MongoDbConfigurationBuilder
  {
    public MongoDbConfigurationBuilder(IServiceCollection services) => this.Services = services;

    public IServiceCollection Services { get; }

    public MongoDbConfigurationBuilder FromAssembly(Assembly assembly)
    {
      foreach ((var type, var _) in MongoDbConfigurationBuilder.GetWithGenericInterface(assembly, typeof (IMongoDbRepository<,>)))
      {
        foreach (var serviceType in type.GetInterfaces().Where((Func<Type, bool>) (i => !i.IsGenericType)))
          this.Services.AddTransient(serviceType, type);
      }
      foreach ((Type type, Type genericType) tuple in MongoDbConfigurationBuilder.GetWithGenericInterface(assembly, typeof (IMongoDbEntityConfiguration<,>)))
      {
        var type = tuple.type;
        this.Services.AddTransient(typeof (IMongoDbEntityConfiguration<,>).MakeGenericType(tuple.genericType), type);
      }
      return this;
    }

    public MongoDbConfigurationBuilder FromAssemblyContaining<T>() => this.FromAssembly(typeof (T).Assembly);

    private static IEnumerable<(Type type, Type genericType)> GetWithGenericInterface(
      Assembly assembly,
      Type genericTypeDefinition)
    {
      return assembly.ExportedTypes.Where(t => t is { IsClass: true, IsAbstract: false, IsInterface: false }).Select((Func<Type, (Type, Type)>) (t =>
      {
          var type = t.GetInterfaces()
              .Where((Func<Type, bool>)(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition))
              .FirstOrDefault();

          return (type ==  null ? (t,null) : (t, type.GenericTypeArguments.First()))!;
      })).Where(_ => true);
    }
  }
}
