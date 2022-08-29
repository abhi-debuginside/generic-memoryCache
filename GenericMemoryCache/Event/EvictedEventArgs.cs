namespace LUSID.Utilities.GenericMemoryCache.Event;

public class EvictedEventArgs : EventArgs
{
    public string Key { get; set; }
    public DateTime EvictedOn { get; set; }
    public object Item { get; set; }
    public DateTime LastAccessedOn { get; set; }

}