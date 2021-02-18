using System.Collections.Generic;

namespace Valuator {
    public interface IStorage
    {
        void StoreValue(string key, string value);
        string GetValue(string key);
        HashSet<string> GetAllValuesWithKeyPrefix(string prefix);
    }
}
