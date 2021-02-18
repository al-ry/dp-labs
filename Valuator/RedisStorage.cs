using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
using System;
namespace Valuator
{
    public class RedisStorage : IStorage
    {
        private readonly ILogger<RedisStorage> _logger;
        private const string _host = "localhost";
        private const int _port = 6379;
        private const string _allTextsKey = "allTextsKey";
        private IConnectionMultiplexer _connection;
        public RedisStorage (ILogger<RedisStorage> logger)
        {
            _connection = ConnectionMultiplexer.Connect($"{_host}, allowAdmin=true");
            _logger = logger;
        }
        public void StoreValue(string key, string value)
        {
            var db = _connection.GetDatabase();
            if (key.StartsWith("TEXT-"))
            {
                db.SetAdd(_allTextsKey, value);
            }
            db.StringSet(key, value);
        }

        public string GetValue(string key)
        {    
            var db = _connection.GetDatabase();
            return db.StringGet(key);
        }
        public bool FindText(string text)
        {
            var db = _connection.GetDatabase();
            return db.SetContains(_allTextsKey, text);
        } 
    }
}