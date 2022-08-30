
# Implementation plan
## Req 1 - Try bare minimum add & get an items
    1. Should be able to check a key is exists in cache - done
    2. Should be able to add any object - done
    3. Should be able to retrieve item from cache - done
    4. Should not cache null value - done
## Req 2 - 
    1. Configurable maximum number of items which it can hold at any one time. - done
    2. If the cache becomes full, any attempts to add additional items should succeed - done
    3. The ‘least recently used’ approach when selecting which item to evict. - done
## Req 3 -
    1. Should be thread-safe for all methods -done
    2. Implement callback, allows the consumer to know when items get evicted - done

# Times
20 aug 22
- 09:40 to 10:01
- 10:03 to 11:00
- 12:00 to 12:32
25 aug 22
- 12:00 to 14:00

29 Aug 22
- 9:00 - 11:30
- 17:00 - 19:00
- 23:00 - 24:30

29 Aug 22
- 08:00 08:20

Total : 12 Hours.

# Basic things
- Unit test
- Thread safe
- Type safe
- Do not cache null value
- To avoid the risk of running out of memory
- the cache will need to have a configurable threshold for the maximum number of items which it can hold at any one time
- If the cache becomes full, any attempts to add additional items should succeed, but will result in another item in the cache being evicted
- should implement the ‘least recently used’ approach when selecting which item to evict.

# Nice to have
- The cache component is intended to be used as a singleton. As such, you should ideally make your component thread-safe for all methods, but you can skip this feature if you run out of time.

- Another useful feature would be some kind of mechanism which allows the consumer to know when items get evicted. Again, if you run out of time, you can skip this feature too.


# Methods to expose

Method 1 bool IsExist(key)
Method 2 IResult Get<IResult>(key)
Method 3 void Set(key, object)



