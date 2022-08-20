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

    [Fact(DisplayName = "Should return true if item key found")]
    public void ShouldReturnTrue_IfItemKeyFound()
    {
        // Arrange
        string key = "key1";
        bool expected = false;

        // Act
        var actual = _cache.IsExists(key);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact(DisplayName = "Should throw error if item key is null")]
    public void ShouldThrowException_IfItemKeyIsNull()
    {
        // Arrange
        string key = null;
        string expected = "Invalid key. Key should be non nullable.";

        // Act & Assert
        var exception = Assert.Throws<System.ArgumentException>(() => _cache.IsExists(key));
        Assert.Equal(expected, exception.Message);
    }
    #endregion
}