using System;
using System.Threading.Tasks;
using LUSID.Utilities.GenericMemoryCache;
using Xunit;
using Xunit.Abstractions;

namespace LUSID.Utilities.GenericMemoryCache.Tests;

public class GenericMemoryCacheTest
{
    private readonly IGenericMemoryCache _cache;
    ITestOutputHelper _output;
    private readonly int _cacheSize = 5;
    public GenericMemoryCacheTest(ITestOutputHelper output)
    {
        _cache = new GenericMemoryCache(maxItemCount: _cacheSize);
        _output = output;
    }

    #region Tests - IsExists

    [Fact(DisplayName = "IsExists - Should return true if item key found")]
    public void IsExists_ShouldReturnTrue_IfItemKeyFound()
    {
        // Arrange
        string key = "key1";
        bool expected = false;

        // Act
        var actual = _cache.IsExists(key);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact(DisplayName = "IsExists - Should throw error if item key is null")]
    public void IsExists_ShouldThrowException_IfItemKeyIsNull()
    {
        // Arrange
        string key = null;
        string expected = "Invalid key. Key should be non nullable.";

        // Act & Assert
        var exception = Assert.Throws<System.ArgumentException>(() => _cache.IsExists(key));
        Assert.Equal(expected, exception.Message);
    }
    #endregion

    #region Tests - Add or update an item to cache
    [Fact(DisplayName = "Set - Should add or update an item to cache")]
    public void Set_ShouldAddOrUpdateAnItemAddedToCache()
    {
        // Arrange
        string key = "key1";
        var entry = Guid.NewGuid();
        var expected = true;

        // Act
        _cache.Set<Guid>(key, entry);

        // Assert
        Assert.Equal(expected, _cache.IsExists(key));
    }

    [Fact(DisplayName = "Set - Should throw error if item key is null")]
    public void Set_ShouldThrowException_IfItemKeyIsNull()
    {
        // Arrange
        string key = null;
        var entry = Guid.NewGuid();
        string expected = "Invalid key. Key should be non nullable.";

        // Act & Assert
        var exception = Assert.Throws<System.ArgumentException>(() => _cache.Set<Guid>(key, entry));
        Assert.Equal(expected, exception.Message);
    }

    [Fact(DisplayName = "Set - Should throw error if item is null")]
    public void Set_ShouldThrowException_IfItemIsNull()
    {
        // Arrange
        string key = "key1";
        Guid? entry = null;
        string expected = "Cache entry cannot be null";

        // Act & Assert
        var exception = Assert.Throws<System.ArgumentException>(() => _cache.Set<Guid?>(key, entry));
        Assert.Equal(expected, exception.Message);
    }

    #endregion

    #region Tests - Get all items from cache
    [Fact(DisplayName = "Get - Should be able to retrieve item from cache")]
    public void Get_ShouldReturnItem_FromCache()
    {
        // Arrange
        string key = "key1";
        var expected = Guid.NewGuid();

        _cache.Set<Guid>(key, expected);

        // Act
        var result = _cache.Get<Guid>(key);

        // Assert
        Assert.IsType<Guid>(result);

        Assert.Equal(expected, result);
    }

    [Fact(DisplayName = "Get - Should throw exception item not found from cache")]
    public void Get_ShouldThrowException_IfItemNotFoundFromCache()
    {
        // Arrange
        string key = "key1";

        // Act
        var result = Record.Exception(() => _cache.Get<Guid>(key));

        // Assert
        Assert.IsType<Exception>(result);

        Assert.Equal("Item not found.", result.Message);
    }
    #endregion

    #region Tests - Cache count
    [Fact(DisplayName = "SizeExceeded - Should return true when the cache item count exceeds")]
    public void Get_ShouldReturnTrue_WhenCacheItemCountExceeds()
    {
        // Arrange & Act
        for (int i = 0; i < 5; i++)
        {
            string key = $"key{i}";
            var value = Guid.NewGuid();
            _cache.Set<Guid>(key, value);
        }

        // Assert
        Assert.True(_cache.SizeExceeded);
    }
    #endregion

    #region Tests - Add additional items should succeed
    [Fact(DisplayName = "Set - Should be able to add items into cache even cache is full.")]
    public void Set_ShouldAddToCache_WhenCacheIsFull1()
    {
        // Arrange
        for (int i = 1; i < 8; i++)
        {
            string key = $"key{i}";
            var value = Guid.NewGuid();
            _cache.Set<Guid>(key, value);
        }

        var newValue = Guid.NewGuid();
        var newKey = "key9";
        _cache.Set<Guid>(newKey, newValue);

        // Act
        var result = _cache.Get<Guid>(newKey);

        // Assert
        Assert.Equal(result, newValue);
    }

    [Fact(DisplayName = "Evit - Should evit least recently used item from cache when cache is full")]
    public void Evit_ShouldEvitLeastRecentlyUsedItem_WhenCacheIsFull()
    {
        // Arrange
        string key1 = "key1";
        string value1 = "key1";
        _cache.Set<string>(key1, value1);
        Task.Delay(new TimeSpan(0, 1, 0));

        string key2 = "key2";
        string value2 = "key2";
        _cache.Set<string>(key2, value2);
        Task.Delay(new TimeSpan(0, 1, 0));

        string key3 = "key3";
        int value3 = 5;
        _cache.Set<int>(key3, value3);
        Task.Delay(new TimeSpan(0, 1, 0));

        string key4 = "key4";
        decimal value4 = 890.675M;
        _cache.Set<decimal>(key4, value4);
        Task.Delay(new TimeSpan(0, 1, 0));

        string key5 = "key5";
        bool value5 = false;
        _cache.Set<bool>(key5, value5);
        Task.Delay(new TimeSpan(0, 1, 0));

        // getting key 1, key 3, key 5
        var gettingValue1 = _cache.Get<string>(key1);
        Task.Delay(new TimeSpan(0, 1, 0));

        var gettingValue3 = _cache.Get<int>(key3);
        Task.Delay(new TimeSpan(0, 1, 0));

        var gettingValue5 = _cache.Get<bool>(key5);
        Task.Delay(new TimeSpan(0, 1, 0));

        // Act
        string key6 = "key6";
        var value6 = "hello";
        _cache.Set<string>(key6, value6);

        // least access will key 2 then key 4.
        var value2Exist = _cache.IsExists(key2);
        var value4Exist = _cache.IsExists(key4);
        var value6Exist = _cache.IsExists(key6);

        foreach (var item in _cache.Removedkeys)
        {
            _output.WriteLine(item);
        }

        // Assert
        Assert.False(value2Exist);
        Assert.True(value4Exist);
        Assert.True(value6Exist);
    }
    #endregion
}