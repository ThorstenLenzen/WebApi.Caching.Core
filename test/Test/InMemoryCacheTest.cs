using System;
using System.Collections.Generic;
using System.Threading;
using Toto.WebApi.Caching.Core.Contracts;
using Toto.WebApi.Caching.Core.Core;
using Xunit;

namespace Toto.WebApi.Caching.Test
{
    /// <summary>
    /// Tests for the <see cref="InMemoryCache"/>
    /// </summary>
    public class InMemoryCacheTest
    {
        private const string TestValue = "test";

        /// <summary>
        /// Tests the in memory cache to add a cache entry.
        /// </summary>
        [Fact]
        public void UseInMemoryCache_AddCacheEntry_StoresEntryInCache()
        {
            ICache cache = new InMemoryCache();
            cache.Add(TestValue, TestValue);
        }

        /// <summary>
        /// Tests the retrieval of a cache entry.
        /// </summary>
        [Fact]
        public void UseInMemoryCache_GetCacheEntry_RetrievesCorrectEntry()
        {
            // Arrange
            ICache cache = new InMemoryCache();
            cache.Add(TestValue, TestValue);

            // Act
            var result = cache.Get<string>(TestValue);

            // Assert
            Assert.Equal(TestValue, result);
        }

        /// <summary>
        /// Tests the retrieval of a cache entry with a non existing name.
        /// </summary>
        [Fact]
        public void UseInMemoryCache_GetCacheEntryWithNonExsistingName_ThrowsKeyNotFoundException()
        {
            // Arrange
            ICache cache = new InMemoryCache();

            // Act
            var exception = Record.Exception(() => cache.Get<string>(TestValue));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }

        /// <summary>
        /// Tests the retrieval of a cache entry with a non existing name.
        /// </summary>
        [Fact]
        public void UseInMemoryCache_RemoveEntry_ThrowsKeyNotFoundExceptionOnCalling()
        {
            // Arrange
            ICache cache = new InMemoryCache();
            cache.Add(TestValue, TestValue);

            // Act
            cache.Remove(TestValue);

            // Assert
            var exception = Record.Exception(() => cache.Get<string>(TestValue));
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }

        /// <summary>
        /// Tests the creation of a session in the cache.
        /// </summary>
        [Fact]
        public void UseInMemoryCache_CreateNewSession_ReturnsCreatedSession()
        {
            // Arrange
            ICache cache = new InMemoryCache();

            // Act
            ISession session = cache.GetOrCreateSession(TestValue);

            // Assert
            Assert.NotNull(session);
        }

        /// <summary>
        /// Tests the creation of a session in the cache.
        /// </summary>
        [Fact]
        public void UseInMemoryCache_GetExistingSession_ReturnsDesiredSession()
        {
            // Arrange
            ICache cache = new InMemoryCache();
            cache.GetOrCreateSession(TestValue);

            // Act
            ISession session = cache.GetOrCreateSession(TestValue);

            // Assert
            Assert.NotNull(session);
            Assert.Equal(TestValue, session.Id);
        }

        /// <summary>
        /// Tests the expiration of a cached item.
        /// </summary>
        [Fact]
        public void UseInMemoryCache_GetExpiredCacheItem_ThrowsKeyNotFoundException()
        {
            // Arrange
            var cache = new InMemoryCache
            {
                CacheExpirationOffset = TimeSpan.FromMilliseconds(2)
            };
            cache.Add(TestValue, TestValue);

            // Act
            Thread.Sleep(5);
            var exception = Record.Exception(() => cache.Get<string>(TestValue));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }

        /// <summary>
        /// Tests the expiration of a session.
        /// </summary>
        [Fact]
        public void UseInMemoryCache_GetExpiredSession_ReturnsNewEmptySession()
        {
            // Arrange
            var cache = new InMemoryCache
            {
                SessionCacheExpirationOffset = TimeSpan.FromMilliseconds(2)
            };
            var session = cache.GetOrCreateSession(TestValue);
            session.Add(TestValue, TestValue);

            // Act
            Thread.Sleep(5);
            session = cache.GetOrCreateSession(TestValue);
            var exception = Record.Exception(() => session.Get<string>(TestValue));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }
    }
}
