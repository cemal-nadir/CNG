using CNG.Core.Exceptions;
using StackExchange.Redis;

namespace CNG.Cache
{
  public class RedisServer : IRedisServer
  {
    private readonly RedisOption _redisOption;
    private ConnectionMultiplexer _connectionMultiplexer;

    public RedisServer(RedisOption redisOption, ConnectionMultiplexer connectionMultiplexer)
    {
      _redisOption = redisOption;
      _connectionMultiplexer = connectionMultiplexer;
      Connect();
    }

    public IServer GetServer()
    {
      var connectionMultiplexer = _connectionMultiplexer;
      var endPoints = _connectionMultiplexer.GetEndPoints();
      var endPoint = endPoints.FirstOrDefault();
      return connectionMultiplexer.GetServer(endPoint);
    }

    public IDatabase GetDb(int db) => _connectionMultiplexer.GetDatabase(db);

    public void FlushDatabase(int db) => _connectionMultiplexer.GetServer(_redisOption.ConnectionString).FlushDatabase(db);

    private void Connect()
    {
      if (string.IsNullOrEmpty(_redisOption.ConnectionString))
        throw new NotFoundException("Redis Connection string is empty");
      try
      {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisOption.ConnectionString);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
      if (_connectionMultiplexer == null)
        throw new NotFoundException("Redis Server Not Connected");
    }
  }
}
