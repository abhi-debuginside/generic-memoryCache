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

    // Create CacheEntry and returns object.
    public CacheEntry Create<IEntry>(IEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentException("Cache entry cannot be null");
        }

        return new CacheEntry(entry, typeof(IEntry));
    }
}