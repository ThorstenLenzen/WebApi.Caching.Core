# WebApi.Caching.Core
A caching implementation using .Net Core to use with WepApi. The project 
exposes both a cache and and a session. It also exposes interfaces to use 
with dependency injection. It consists of two projects:

- Contracts includes the used interfaces
- Core includes an in memory implementation of the cache and the session.


## Setting up the Cache
Caching can be established e.g. by configuring an ICache implementation with
a DI Container. As an example the following uses SimpleInjector.

```csharp
// Setting up the container
Container container = new Container();

// Registering the cache
container.Register<ICache, InMemoryCache>();

// Resolving the cache
ICache cache = container.GetInstance<ICache>();
```

## Using the Cache
To use the cache you can add, remove and get items from the cache.

```csharp
// Setup some variables
string itemName = "cached Item";
MyObject item = new MyObject();

// Add an item
cache.Add(itemName, item);

// Get an item
MyObject item = cache.Get<MyObject>();

// Remove an item
cache.Remove(itemName);
```

## Setting the Expiration for an Item
The standard expiration for an item in the cache is set to one day. To change 
hat, the InMemoryCache exposes an `CacheExpirationOffset` of type `TimeSpan`.
It is not exposed, via the interface, because a cache consumer has no need to
to change that expiration time. It can, however, be changed during the 
configuration of the DI Container. An example using SimpleInjector is written
below.

```csharp
// Setting up the cache
InMemoryCache cache = new InMemoryCache();

// Setting the expiration to 2 hours after 
// adding the item to the cache
cache.CacheExpirationOffset = TimeSpan.FromHours(2);

// Setting uo the container and registering
// the cache
Container container = new Container();
container.RegisterInstance<ICache>(cache);

// Resolving the cache
ICache cache = container.GetInstance<ICache>();
```

## Using the Sesssion provided by the Cache
