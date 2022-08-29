using System;
using LUSID.Utilities.GenericMemoryCache.Event;

namespace LUSID.Utilities.GenericMemoryCache;

public interface IGenericMemoryCache
{
    int MaxItemCount { get; }

    bool SizeExceeded { get; }

    //
    // Summary:
    //     Return TRUE if an item key found in cache.
    bool IsExists(string key);

    //
    // Summary:
    //     Add or update an item into cache.
    void Set<IEntry>(string key, IEntry entry);

    //
    // Summary:
    //     Get an item from cache based on key .
    IEntry Get<IEntry>(string key);

    //
    // Summary:
    //     Event will raise when an item gets evicted from cache.
    event EventHandler<EvictedEventArgs> ItemEvicted;
}