using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LUSID.Utilities.GenericMemoryCache.Tests;

public class GenericMemoryCacheThreadsafeTest
{
    ITestOutputHelper _output;
    public GenericMemoryCacheThreadsafeTest(ITestOutputHelper output)
    {
        _output = output;
    }

    #region Tests - Should be thread-safe for all methods
    [Fact(DisplayName = "Set - Should be able to add items into cache in thread safe1")]
    public void Set_ShouldAddToCache_InThreadSafe()
    {
        // Arrange
        var _cache = new GenericMemoryCache(1500);
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

        foreach (var item in _cache.Removedkeys)
        {
            _output.WriteLine(item);
        }
        // Assert
        Assert.True(_cache.IsExists("batch1_499"));
        Assert.True(_cache.IsExists("batch2_499"));
        Assert.True(_cache.IsExists("batch3_700"));
        Assert.True(_cache.IsExists("batch1_100"));
        Assert.Equal(200, _cache.Removedkeys.Count);
    }

    [Fact(DisplayName = "Get - Should be able to get items into cache in thread safe")]
    public void Get_ShouldFetchFromCache_InThreadSafe()
    {
        // Arrange
        var _cache = new GenericMemoryCache(1500);
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
        Assert.Equal(0, _cache.Removedkeys.Count);
    }

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
            _output.WriteLine($"key: {key}, value: {item}");
        }
    }
    #endregion
}