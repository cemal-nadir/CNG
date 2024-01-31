
namespace CNG.Cache
{
  public class RedisOption
  {
    public RedisOption(
      string instanceName,
      string identityName,
      string connectionString,
      int? absoluteExpiration=null)
    {
      InstanceName = instanceName;
      ConnectionString = connectionString;
      AbsoluteExpiration = absoluteExpiration;
      IdentityName = identityName;
    }

    public string InstanceName { get; }

    public string IdentityName { get; }

    public string ConnectionString { get; }

    public int? AbsoluteExpiration { get; }
  }
}
