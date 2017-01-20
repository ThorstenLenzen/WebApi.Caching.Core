using System;
using System.Collections.Generic;
using Microsoft.Framework.Caching.Memory;
using Toto.WebApi.Caching.Core.Contracts;

namespace Toto.WebApi.Caching.Core.Core
{
    /// <summary>
    /// Represents an in memory instance of an <see cref="ICache"/> implementation.
    /// </summary>
    public class InMemoryCache : ICache
    {
        private readonly object _cacheLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCache"/> class.
        /// </summary>
        public InMemoryCache()
        {
            _cacheLock = new object();

            CacheExpirationOffset = TimeSpan.FromDays(1);
            SessionCacheExpirationOffset = TimeSpan.FromDays(1);

            SessionCache = new MemoryCache(new MemoryCacheOptions());
            Cache = new MemoryCache(new MemoryCacheOptions());
        }

        /// <summary>
        /// Gets or sets the expiration offset for the cache.
        /// </summary>
        /// <value>
        /// The cache expiration offset.
        /// </value>
        public TimeSpan CacheExpirationOffset { get; set; }

        /// <summary>
        /// Gets or sets the expiration offset for the session cache.
        /// </summary>
        /// <value>
        /// The session cache expiration offset.
        /// </value>
        public TimeSpan SessionCacheExpirationOffset { get; set; }

        /// <summary>
        /// Gets the <see cref="MemoryCache"/> which contains the stored sessions.
        /// </summary>
        /// <value>
        /// The dictionary.
        /// </value>
        protected MemoryCache SessionCache { get; }

        /// <summary>
        /// Gets the <see cref="MemoryCache"/> which contains the items stored in the cache.
        /// </summary>
        /// <value>
        /// The dictionary.
        /// </value>
        protected MemoryCache Cache { get; }

        /// <summary>
        /// Gets the or creates the session for the specified id, if it not already exists.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>A <see cref="ISession"/>An instance for the specified id.</returns>
        public ISession Session(string sessionId)
        {
            ISession session;

            bool sessionExists = SessionCache.TryGetValue(sessionId, out session);

            if (!sessionExists)
            {
                session = new InMemorySession(sessionId);
                lock (_cacheLock)
                {
                    var options = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = SessionCacheExpirationOffset
                    };

                    SessionCache.Set(sessionId, session, options);
                }
            }
            else
            {
                session = SessionCache.Get<ISession>(sessionId);
            }

            return session;
        }

        /// <summary>
        /// Adds an item with the specified name to the cache.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="value">The item value.</param>
        public void Add(string name, object value)
        {
            lock (_cacheLock)
            {
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheExpirationOffset,

                };

                Cache.Set(name, value, options);
            }
        }

        /// <summary>
        /// Removes an item with the specified name from the cache.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public void Remove(string name)
        {
            object entry;
            bool entryExists = Cache.TryGetValue(name, out entry);

            if (!entryExists)
            {
                throw new KeyNotFoundException();
            }

            lock (_cacheLock)
            {
                Cache.Remove(name);
            }
        }

        /// <summary>
        /// Retrieves an item with the specified name from the cache.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name of the item.</param>
        /// <returns>The item specified by its name.</returns>
        public TValue Get<TValue>(string name)
        {
            object entry;
            bool entryExists = Cache.TryGetValue(name, out entry);

            if (!entryExists)
            {
                throw new KeyNotFoundException();
            }

            return Cache.Get<TValue>(name);
        }
    }
}
