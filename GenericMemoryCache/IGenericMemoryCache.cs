using System;

namespace LUSID.Utilities.GenericMemoryCache;

public interface IGenericMemoryCache
{
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

    int MaxItemCount { get; set; }

    bool SizeExceeded { get; }

    List<string> Removedkeys { get; set; }
}