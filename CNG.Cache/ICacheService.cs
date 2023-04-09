
namespace CNG.Cache
{
    public interface ICacheService
    {
        Task<bool> AnyAsync(string key, int db = 0);

        Task<bool> AnyAsync(string instanceName, string key, int db = 0);

        Task<T> GetAsync<T>(string key, int db = 0) where T : new();

        Task<T> GetAsync<T>(string instanceName, string key, int db = 0) where T : new();

        Task<string> GetAsync(string key, int db = 0);

        Task<string> GetAsync(string instanceName, string key, int db = 0);

        Task SetAsync(string key, string data, int? minute = null, int db = 0);

        Task SetAsync(string instanceName, string key, string data, int? minute = null, int db = 0);

        Task RemoveAsync(string key, int db = 0);

        Task RemoveAsync(string instanceName, string key, int db = 0);

        Task RemoveByPatternAsync(string pattern, int db = 0);

        Task RemoveByPatternAsync(string instanceName, string pattern, int db = 0);

        void Clear();
    }
}
