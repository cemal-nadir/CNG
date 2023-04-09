#nullable enable
namespace CNG.Cap
{
  public class CapOption
  {
    public CapOption(
      string groupName,
      string version,
      CapOption.Mongo mongoOption,
      CapOption.RabbitMq rabbitMqOption,
      string? dashboardUrl = null)
    {
      this.GroupName = groupName;
      this.Version = version;
      this.MongoOption = mongoOption;
      this.RabbitMqOption = rabbitMqOption;
      this.DashboardUrl = dashboardUrl;
    }

    public CapOption(
      string groupName,
      string version,
      string mongoDbConnectionString,
      string mongoDbName,
      CapOption.RabbitMq rabbitMqOption,
      string? dashboardUrl = null)
    {
      this.GroupName = groupName;
      this.Version = version;
      this.MongoOption = new CapOption.Mongo(mongoDbConnectionString, mongoDbName);
      this.RabbitMqOption = rabbitMqOption;
      this.DashboardUrl = dashboardUrl;
    }

    public string GroupName { get; }

    public string Version { get; }

    public string? DashboardUrl { get; }

    public CapOption.Mongo MongoOption { get; }

    public CapOption.RabbitMq RabbitMqOption { get; }

    public int FailedRetryCount { get; set; } = 1;

    public class RabbitMq
    {
      public RabbitMq(string host, string userName, string password, int port)
      {
        this.Host = host;
        this.UserName = userName;
        this.Password = password;
        this.Port = port;
      }

      public string Host { get; }

      public string UserName { get; }

      public string Password { get; }

      public int Port { get; }
    }

    public class Mongo
    {
      public Mongo(string host, string dbName, string userName, string password, int port)
      {
        this.Host = host;
        this.DbName = dbName;
        this.UserName = userName;
        this.Password = password;
        this.Port = port;
        this.ConnectionString = string.Empty;
      }

      public Mongo(string connectionString, string dbName)
      {
        this.ConnectionString = connectionString;
        this.Host = string.Empty;
        this.DbName = dbName;
        this.UserName = string.Empty;
        this.Password = string.Empty;
      }

      public string ConnectionString { get; }

      public string Host { get; }

      public string DbName { get; }

      public string UserName { get; }

      public string Password { get; }

      public int Port { get; }
    }
  }
}
