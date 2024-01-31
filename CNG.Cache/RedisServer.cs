using StackExchange.Redis;

namespace CNG.Cache
{
  public class RedisServer : IRedisServer
  {
    private readonly RedisOption _redisOption;
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public RedisServer(RedisOption redisOption, ConnectionMultiplexer connectionMultiplexer)
    {
      _redisOption = redisOption;
      _connectionMultiplexer = connectionMultiplexer;
    }

    public IServer GetServer()
    {
      var endPoints = _connectionMultiplexer.GetEndPoints();
      var endPoint = endPoints.FirstOrDefault();
      return _connectionMultiplexer.GetServer(endPoint);
    }

    public IDatabase GetDb(int db) => _connectionMultiplexer.GetDatabase(db);

    public void FlushDatabase(int db) => _connectionMultiplexer.GetServer(_redisOption.ConnectionString).FlushDatabase(db);
  }
}
