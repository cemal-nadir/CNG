
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace CNG.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly RedisOption _option;
        private readonly IRedisServer _server;

        public RedisCacheService(IRedisServer server, RedisOption option)
        {
            _server = server;
            _option = option;
        }

        public async Task<bool> AnyAsync(string? key, int db = 0)
        {
            var database = _server.GetDb(db);
            RedisValue redisValue = await database.StringGetAsync(new RedisKey(_option.InstanceName + "." + key));
            var result = redisValue;
            bool hasValue = result.HasValue;
            return hasValue;
        }

        public async Task<bool> AnyAsync(string instanceName, string key, int db = 0)
        {
            var database = _server.GetDb(db);
            RedisValue redisValue = await database.StringGetAsync(new RedisKey(instanceName + "." + key));
            var result = redisValue;
            bool hasValue = result.HasValue;
            return hasValue;
        }

        public async Task<T> GetAsync<T>(string key, int db = 0) where T : new()
        {
            var database = _server.GetDb(db);
            RedisValue redisValue = await database.StringGetAsync(new RedisKey(_option.InstanceName + "." + key));
            var result = redisValue;
            T obj;
            if (result.HasValue)
                obj = JsonConvert.DeserializeObject<T>(result.ToString(), new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }) ?? new T();
            else
                obj = new T();
            var async = obj;
            return async;
        }

        public async Task<T> GetAsync<T>(string instanceName, string key, int db = 0) where T : new()
        {
            var database = _server.GetDb(db);
            RedisValue redisValue = await database.StringGetAsync(new RedisKey(instanceName + "." + key));
            var result = redisValue;
            T obj;
            if (result.HasValue)
                obj = JsonConvert.DeserializeObject<T>(result.ToString(), new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }) ?? new T();
            else
                obj = new T();
            var async = obj;
            return async;
        }

        public async Task<string> GetAsync(string? key, int db = 0)
        {
            var database = _server.GetDb(db);
            RedisValue redisValue = await database.StringGetAsync(new RedisKey(_option.InstanceName + "." + key));
            var result = redisValue;
            string async = !result.HasValue ? "" : result.ToString();
            return async;
        }

        public async Task<string> GetAsync(string instanceName, string key, int db = 0)
        {
            var database = _server.GetDb(db);
            RedisValue redisValue = await database.StringGetAsync(new RedisKey(instanceName + "." + key));
            var result = redisValue;
            string async = !result.HasValue ? "" : result.ToString();
            return async;
        }

        public async Task SetAsync(string? key, string data, int? minute = null, int db = 0)
        {
            minute ??= _option.AbsoluteExpiration;
            TimeSpan? ts = minute is null ? null : TimeSpan.FromMinutes(minute.Value);
            var database = _server.GetDb(db);
            await RemoveAsync(key, db);
            await database.StringSetAsync(new RedisKey(_option.InstanceName + "." + key), new RedisValue(data), ts);
        }

        public async Task SetAsync(string instanceName, string? key, string data, int? minute = null, int db = 0)
        {
            minute ??= _option.AbsoluteExpiration;
            TimeSpan? ts = minute is null ? null : TimeSpan.FromMinutes(minute.Value);
			var database = _server.GetDb(db);
            await RemoveAsync(key, db);
            await database.StringSetAsync(new RedisKey(instanceName + "." + key),new RedisValue(data), ts);
        }
        public async Task SetAsync<T>(string? key, T data, int? minute = null, int db = 0) where T : class
        {
			minute ??= _option.AbsoluteExpiration;
			TimeSpan? ts = minute is null ? null : TimeSpan.FromMinutes(minute.Value);
			var database = _server.GetDb(db);
	        await RemoveAsync(key, db);
	        var dataString = JsonConvert.SerializeObject(data);
	        await database.StringSetAsync(new RedisKey(_option.InstanceName + "." + key), new RedisValue(dataString), ts);
        }

        public async Task SetAsync<T>(string instanceName, string? key, T data, int? minute = null, int db = 0) where T : class
		{
			minute ??= _option.AbsoluteExpiration;
			TimeSpan? ts = minute is null ? null : TimeSpan.FromMinutes(minute.Value);
			var database = _server.GetDb(db);
	        await RemoveAsync(key, db);
	        var dataString = JsonConvert.SerializeObject(data);
			await database.StringSetAsync(new RedisKey(instanceName + "." + key), new RedisValue(dataString), ts);
        }
		public async Task RemoveAsync(string? key, int db = 0)
        {
            var database = _server.GetDb(db);
            var flag = await AnyAsync(key, db);
            if (flag)
            {
                 await database.KeyDeleteAsync(new RedisKey(_option.InstanceName + "." + key));
            }
        }

        public async Task RemoveAsync(string instanceName, string key, int db = 0)
        {
            var database = _server.GetDb(db);
            var flag = await AnyAsync(instanceName, key, db);
            if (flag)
            {
                await database.KeyDeleteAsync(new RedisKey(instanceName + "." + key));
            }
        }

        public async Task RemoveByPatternAsync(string pattern, int db = 0)
        {
            var database = _server.GetDb(db);
            var server = _server.GetServer();
            RedisValue redisValue1 = new RedisValue(_option.InstanceName + "." + pattern + "*");
            var keys = server.Keys(db, redisValue1).ToList();
            foreach (var redisKey in keys)
            {
                await database.KeyDeleteAsync(redisKey);
            }
            await database.KeyDeleteAsync(new RedisKey(_option.InstanceName + "." + pattern));
        }

        public async Task RemoveByPatternAsync(string instanceName, string pattern, int db = 0)
        {
            var database = _server.GetDb(db);
            var server = _server.GetServer();
            RedisValue redisValue1 = new RedisValue(instanceName + "." + pattern + "*");
            var keys = server.Keys(db, redisValue1).ToList();
            foreach (var redisKey in keys)
            {
                await database.KeyDeleteAsync(redisKey);
            }
            await database.KeyDeleteAsync(new RedisKey(instanceName + "." + pattern));
        }

        public void Clear()
        {
            for (var db = 0; db <= 15; ++db)
                _server.FlushDatabase(db);
        }
    }
}
