namespace LUSID.Utilities.GenericMemoryCache;

public class CacheEntry
{
    private readonly object cacheLock = new object();
    private CacheEntry(object item)
    {
        Item = item;
    }
    public object Item { get; set; }
    public DateTime LastAccessedOn { get; set; }
    public DateTime AddedOn { get; set; }

    public IEntry Get<IEntry>()
    {
        lock (cacheLock)
        {
            LastAccessedOn = DateTime.UtcNow;
            return (IEntry)Item;
        }
    }

    // Create CacheEntry and returns object.
    public static CacheEntry Create<IEntry>(IEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentException("Cache entry cannot be null");
        }
        var cacheEntry = new CacheEntry(entry);
        var now = DateTime.UtcNow;
        cacheEntry.AddedOn = now;
        cacheEntry.LastAccessedOn = now;

        return cacheEntry;
    }
}