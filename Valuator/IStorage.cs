using System.Collections.Generic;

namespace Valuator {
    public interface IStorage
    {
        void StoreValue(string key, string value);
        string GetValue(string key);
        List<string> GetAllValuesWithKeyPrefix(string prefix);
    }
}
