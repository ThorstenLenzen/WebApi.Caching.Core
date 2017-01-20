using System;
using System.Collections.Generic;
using Toto.WebApi.Caching.Core.Contracts;
using Toto.WebApi.Caching.Core.Core;
using Xunit;

namespace Toto.WebApi.Caching.Test
{
    /// <summary>
    /// Tests for the <see cref="InMemorySession"/>
    /// </summary>
    public class InMemorySessionTest
    {
        private const string TestValue = "test";

        /// <summary>
        /// Tests the in memory session to add a session entry.
        /// </summary>
        [Fact]
        public void UseInMemorySession_AddSessionEntry_StoresEntryInSession()
        {
            ISession session = new InMemorySession(Guid.NewGuid().ToString());
            session.Add(TestValue, TestValue);
        }

        /// <summary>
        /// Tests the retrieval of a session entry.
        /// </summary>
        [Fact]
        public void UseInMemorySession_GetSessionEntry_RetrievesCorrectEntry()
        {
            // Arrange
            ISession session = new InMemorySession(Guid.NewGuid().ToString());
            session.Add(TestValue, TestValue);

            // Act
            var result = session.Get<string>(TestValue);

            // Assert
            Assert.Equal(TestValue, result);
        }

        /// <summary>
        /// Tests the retrieval of a session entry with a non existing name.
        /// </summary>
        [Fact]
        public void UseInMemorySession_GetSessionEntryWithNonExsistingName_ThrowsKeyNotFoundException()
        {
            // Arrange
            ISession session = new InMemorySession(Guid.NewGuid().ToString());

            // Act
            var exception = Record.Exception(() => session.Get<string>(TestValue));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }

        /// <summary>
        /// Tests the retrieval of a session entry with a non existing name.
        /// </summary>
        [Fact]
        public void UseInMemorySession_RemoveEntry_ThrowsKeyNotFoundExceptionOnCalling()
        {
            // Arrange
            ISession session = new InMemorySession(Guid.NewGuid().ToString());
            session.Add(TestValue, TestValue);

            // Act
            session.Remove(TestValue);
            var exception = Record.Exception(() => session.Get<string>(TestValue));


            // Assert
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }
    }
}
