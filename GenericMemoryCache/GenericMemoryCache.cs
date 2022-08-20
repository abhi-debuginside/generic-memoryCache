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

    public void Set<IEntry>(string key, IEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentException("Cache entry cannot be null");
        }
        var cacheEntry = CacheEntry.Create<IEntry>(entry);

        if (IsExists(key))
        {
            // remove and add
            _cache.Remove(key);
            _cache.Add(key, cacheEntry);
        }
        else
        {
            // create a new entry
            _cache.Add(key, cacheEntry);
        }
    }
}
