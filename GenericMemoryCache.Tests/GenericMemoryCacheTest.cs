using System;
using LUSID.Utilities.GenericMemoryCache;
using Xunit;

namespace LUSID.Utilities.GenericMemoryCache.Tests;

public class GenericMemoryCacheTest
{
    private readonly IGenericMemoryCache _cache;
    public GenericMemoryCacheTest()
    {
        _cache = new GenericMemoryCache();
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
}