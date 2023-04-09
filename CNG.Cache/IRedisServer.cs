
using StackExchange.Redis;

namespace CNG.Cache
{
  public interface IRedisServer
  {
    IServer GetServer();

    IDatabase GetDb(int db);

    void FlushDatabase(int db);
  }
}
