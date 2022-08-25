namespace LUSID.Utilities.GenericMemoryCache;
public class GenericMemoryCache : IGenericMemoryCache
{
    Dictionary<string, CacheEntry> _cache = new Dictionary<string, CacheEntry>();
    private readonly Object _cacheLock = new Object();

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

    public List<string> Removedkeys { get; set; } = new List<string>();

    public bool IsExists(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Invalid key. Key should be non nullable.");
        }
        lock (_cacheLock)
        {
            return _cache.ContainsKey(key.ToLowerInvariant()) ? true : false;
        }
    }

    public void Set<IEntry>(string key, IEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentException("Cache entry cannot be null");
        }

        lock (_cacheLock)
        {
            var cacheEntry = CacheEntry.Create<IEntry>(entry);

            if (SizeExceeded)
            {
                // remove least accessed item from cache. so recently accessed item should stay in cache.
                var leastAccessedItem = _cache.OrderBy(c => c.Value.LastAccessedOn)?.FirstOrDefault();
                if (leastAccessedItem.HasValue)
                {
                    var message = $"Key: {leastAccessedItem.Value.Key}, Created:{leastAccessedItem.Value.Value.AddedOn.ToLongTimeString()}, Accessed: {leastAccessedItem.Value.Value.LastAccessedOn.ToLongTimeString()}";
                    Removedkeys.Add(message);
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
