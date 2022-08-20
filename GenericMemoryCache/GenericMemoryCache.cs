namespace LUSID.Utilities.GenericMemoryCache;
public class GenericMemoryCache : IGenericMemoryCache
{
    Dictionary<string, CacheEntry> _cache = new Dictionary<string, CacheEntry>();

    public bool IsExists(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Invalid key. Key should be non nullable.");
        }

        return _cache.ContainsKey(key.ToLowerInvariant()) ? true : false;
    }

    
}
