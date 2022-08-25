namespace LUSID.Utilities.GenericMemoryCache;

public class CacheEntry
{
    private CacheEntry(object item, Type itemType)
    {
        Item = item;
        ItemType = itemType;
    }
    public object Item { get; set; }
    public Type ItemType { get; set; }
    public DateTime LastAccessedOn { get; set; }
    public DateTime AddedOn { get; set; }

    public IEntry Get<IEntry>()
    {
        LastAccessedOn = DateTime.UtcNow;
        return (IEntry)Item;
    }

    // Create CacheEntry and returns object.
    public static CacheEntry Create<IEntry>(IEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentException("Cache entry cannot be null");
        }
        var cacheEntry = new CacheEntry(entry, typeof(IEntry));
        var now = DateTime.UtcNow;
        cacheEntry.AddedOn = now;
        cacheEntry.LastAccessedOn = now;

        return cacheEntry;
    }
}