namespace LUSID.Utilities.GenericMemoryCache;
public class GenericMemoryCache : IGenericMemoryCache
{
    Dictionary<string, CacheEntry> _cache = new Dictionary<string, CacheEntry>();

    public GenericMemoryCache()
    {

    }

    public GenericMemoryCache(int maxItemCount)
    {
        MaxItemCount = maxItemCount;
    }

    public int MaxItemCount { get; set; } = 50;

    public bool SizeExceeded
    {
        get
        {
            return _cache.Count() >= MaxItemCount ? true : false;
        }
    }

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

        if (SizeExceeded)
        {
            // remove least accessed item from cache. so recently accessed item should stay in cache.
            var leastAccessedItem = _cache.OrderBy(c => c.Value.LastAccessedOn)?.FirstOrDefault();
            if (leastAccessedItem.HasValue)
            {
                _cache.Remove(leastAccessedItem.Value.Key);
            }
        }

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

    public IEntry Get<IEntry>(string key)
    {
        if (IsExists(key))
        {
            var cacheEntry = _cache.GetValueOrDefault(key);
            if (cacheEntry != null)
            {
                return cacheEntry.Get<IEntry>();
            }
        }

        throw new Exception("Item not found.");
    }
}
