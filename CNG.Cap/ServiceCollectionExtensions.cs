
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CNG.Cap
{
  public static class ServiceCollectionExtensions
  {
    public static void AddCapService(this IServiceCollection services, CapOption options)
    {
      string str1;
      if (!string.IsNullOrEmpty(options.MongoOption.ConnectionString))
      {
        str1 = options.MongoOption.ConnectionString;
      }
      else
      {
        var interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(12, 4);
        interpolatedStringHandler1.AppendLiteral("mongodb://");
        interpolatedStringHandler1.AppendFormatted(options.MongoOption.UserName);
        interpolatedStringHandler1.AppendLiteral(":");
        interpolatedStringHandler1.AppendFormatted(options.MongoOption.Password);
        interpolatedStringHandler1.AppendLiteral("@");
        interpolatedStringHandler1.AppendFormatted(options.MongoOption.Host);
        ref var local = ref interpolatedStringHandler1;
        string str2;
        if (options.MongoOption.Port <= 0)
        {
          str2 = "";
        }
        else
        {
          var interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(1, 1);
          interpolatedStringHandler2.AppendLiteral(":");
          interpolatedStringHandler2.AppendFormatted<int>(options.MongoOption.Port);
          str2 = interpolatedStringHandler2.ToStringAndClear();
        }
        local.AppendFormatted(str2);
        str1 = interpolatedStringHandler1.ToStringAndClear();
      }
      var mongoConnection = str1;
      services.AddSingleton<IMongoClient>(new MongoClient(mongoConnection));
      services.AddCap(x =>
      {
          x.Version = options.Version;
          x.FailedRetryCount = options.FailedRetryCount;
          x.DefaultGroupName = options.GroupName;
          x.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
          x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
          x.UseMongoDB(y =>
          {
              y.DatabaseConnection = mongoConnection;
              y.DatabaseName = options.MongoOption.DbName ?? "";
          });
          x.UseRabbitMQ(y =>
          {
              y.HostName = options.RabbitMqOption.Host ?? "";
              y.UserName = options.RabbitMqOption.UserName ?? "";
              y.Password = options.RabbitMqOption.Password ?? "";
              y.Port = options.RabbitMqOption.Port;
          });
          if (string.IsNullOrWhiteSpace(options.DashboardUrl))
              return;
          x.UseDashboard(opt =>
              opt.PathMatch = ("/" + options.DashboardUrl).Replace("//", "/"));
      });
      Console.WriteLine("### Cap Service is installed");
    }

    public static void AddCapService(this IServiceCollection services, Action<CapOptions> options)
    {
      services.AddCap(options);
      Console.WriteLine("### Cap Service is installed");
    }
  }
}
