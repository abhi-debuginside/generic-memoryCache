using System.Collections.Concurrent;
using LUSID.Utilities.GenericMemoryCache.Event;

namespace LUSID.Utilities.GenericMemoryCache;

public class GenericMemoryCache : IGenericMemoryCache
{
    ConcurrentDictionary<string, CacheEntry> _cache = new ConcurrentDictionary<string, CacheEntry>();

    public GenericMemoryCache(int maxItemCount)
    {
        MaxItemCount = maxItemCount;
    }

    public int MaxItemCount { get; private set; }

    public bool SizeExceeded
    {
        get
        {
            return _cache.Count() >= MaxItemCount ? true : false;
        }
    }

    public event EventHandler<EvictedEventArgs> ItemEvicted;

    public bool IsExists(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Invalid key. Key should be non nullable.");
        }

        return _cache.TryGetValue(key, out CacheEntry cacheEntry);
    }

    public void Set<IEntry>(string key, IEntry entry)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Invalid key. Key should be non nullable.");
        }

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
                // get values and raise item evicted event.
                if (_cache.TryRemove(leastAccessedItem.Value.Key, out CacheEntry cacheEntry1))
                {
                    var eventArgs = new EvictedEventArgs
                    {
                        EvictedOn = DateTime.UtcNow,
                        Key = leastAccessedItem.Value.Key,
                        Item = cacheEntry1.Item,
                        LastAccessedOn = cacheEntry1.LastAccessedOn,
                    };

                    OnItemEvicted(eventArgs);
                }
            }
        }

        // add an item not exists.
        if (!_cache.TryAdd(key, cacheEntry))
        {
            // else Update as new 
            _cache[key] = cacheEntry;
        }
    }

    public IEntry Get<IEntry>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Invalid key. Key should be non nullable.");
        }

        if (_cache.TryGetValue(key, out CacheEntry cacheEntry))
        {
            return cacheEntry.Get<IEntry>();
        }

        throw new Exception($"Item not found. key: {key}");
    }

    protected virtual void OnItemEvicted(EvictedEventArgs e)
    {
        EventHandler<EvictedEventArgs> handler = ItemEvicted;
        if (handler != null)
        {
            handler(this, e);
        }
    }
}
