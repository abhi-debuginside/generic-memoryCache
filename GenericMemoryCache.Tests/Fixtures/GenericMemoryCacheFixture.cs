namespace LUSID.Utilities.GenericMemoryCache.Tests.Fixtures;

public class GenericMemoryCacheFixture
{
    public GenericMemoryCacheFixture()
    {
        GenericMemoryCache = CreateInstance(Constants.MAXITEMCOUNT);
    }

    public IGenericMemoryCache GenericMemoryCache { get; set; }

    public void SetGenericMemoryCache(int maxItemCount)
    {
        GenericMemoryCache = CreateInstance(maxItemCount);
    }

    private IGenericMemoryCache CreateInstance(int maxItemCount)
    {
        return new GenericMemoryCache(maxItemCount);
    }
}