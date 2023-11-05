using CNG.EntityFrameworkCore.Enums;

namespace CNG.EntityFrameworkCore.Models
{
  public class DatabaseOption
  {
    public DatabaseOption(
      DatabaseType databaseType,
      string host,
      string dbName,
      string userName,
      string password, int? port = null)
    {
      this.DatabaseType = databaseType;
      this.Host = host;
      this.DbName = dbName;
      this.UserName = userName;
      this.Password = password;
      this.Port = port;
      this.ConnectionString = string.Empty;
    }

    public DatabaseOption(DatabaseType databaseType, string connectionString)
    {
      this.DatabaseType = databaseType;
      this.Host = string.Empty;
      this.DbName = string.Empty;
      this.UserName = string.Empty;
      this.Password = string.Empty;
      this.Port = new int?();
      this.ConnectionString = connectionString;
    }

    public DatabaseType DatabaseType { get; }

    public string ConnectionString { get; }

    public string Host { get; }

    public string DbName { get; }

    public string UserName { get; }

    public string Password { get; }

    public int? Port { get; }
  }
}
