using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
namespace Valuator
{
    public class RedisStorage : IStorage
    {
        private readonly ILogger<RedisStorage> _logger;
        private IConnectionMultiplexer _connection;
        public RedisStorage (ILogger<RedisStorage> logger)
        {
            _connection = ConnectionMultiplexer.Connect("localhost, allowAdmin=true");
            _logger = logger;
        }
        public void StoreValue(string key, string value)
        {
            var db = _connection.GetDatabase();
            db.StringSet(key, value);
        }
        public string GetValue(string key)
        {    
            var db = _connection.GetDatabase();
            return db.StringGet(key);
        }

        public List<string> GetAllValuesWithKeyPrefix(string prefix)
        {
            var server = _connection.GetServer("localhost", 6379);
            List<string> values = new List<string>();
            foreach (var key in server.Keys(pattern: "*" + prefix + "*"))
            {
                values.Add(this.GetValue(key));
            }
            return values;
        }
    }
}