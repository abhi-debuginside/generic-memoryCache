using System;
using System.Threading;
using LUSID.Utilities.GenericMemoryCache.Tests.Fixtures;
using Xunit;

namespace LUSID.Utilities.GenericMemoryCache.Tests;

[Collection("GenericMemoryCacheTest")]
public class GenericMemoryCacheThreadsafeTest : IClassFixture<GenericMemoryCacheFixture>
{
    private GenericMemoryCacheFixture _fixture;

    public GenericMemoryCacheThreadsafeTest(GenericMemoryCacheFixture fixture)
    {
        _fixture = fixture;
    }

    #region Tests - Should be thread-safe for all methods
    [Fact(DisplayName = "Set - Should be able to add items into cache in thread safe")]
    public void Set_ShouldAddToCache_InThreadSafe()
    {
        // Arrange
        _fixture.SetGenericMemoryCache(1500);
        var _cache = _fixture.GenericMemoryCache;
        var thread1 = new Thread(() => AddItems(_cache, "batch1_"));
        var thread2 = new Thread(() => AddItems(_cache, "batch2_"));
        var thread3 = new Thread(() => AddItems(_cache, "batch3_", 700));

        // Act
        thread1.Start();
        thread2.Start();
        thread3.Start();

        thread1.Join();
        thread2.Join();
        thread3.Join();

        // Assert
        Assert.True(_cache.SizeExceeded);
    }

    [Fact(DisplayName = "Get - Should be able to get items from cache in thread safe")]
    public void Get_ShouldFetchFromCache_InThreadSafe()
    {
        // Arrange
        _fixture.SetGenericMemoryCache(1500);
        var _cache = _fixture.GenericMemoryCache;
        AddItems(_cache, "batch1_", 1200);

        var thread1 = new Thread(() => GetItems(_cache, "batch1_", 1, 700));
        var thread2 = new Thread(() => GetItems(_cache, "batch1_", 701, 900));
        var thread3 = new Thread(() => GetItems(_cache, "batch1_", 901, 1200));

        // Act
        thread1.Start();
        thread2.Start();
        thread3.Start();

        thread1.Join();
        thread2.Join();
        thread3.Join();

        // Assert
        Assert.True(_cache.IsExists("batch1_499"));
        Assert.True(_cache.IsExists("batch1_700"));
        Assert.True(_cache.IsExists("batch1_900"));
        Assert.False(_cache.IsExists("batch1_1201"));
    }
    #endregion

    private void AddItems(IGenericMemoryCache cache, string keyTemplate, int itemCount = 500)
    {
        for (int i = 1; i <= itemCount; i++)
        {
            cache.Set<Guid>($"{keyTemplate}{i}", Guid.NewGuid());
        }
    }

    private void GetItems(IGenericMemoryCache cache, string keyTemplate, int startIndex, int itemCount)
    {
        for (int i = startIndex; i <= itemCount; i++)
        {
            var key = $"{keyTemplate}{i}";
            var item = cache.Get<Guid>(key);
        }
    }
}