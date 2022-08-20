using System;

namespace LUSID.Utilities.GenericMemoryCache;

public interface IGenericMemoryCache
{
    bool IsExists(string key);
}