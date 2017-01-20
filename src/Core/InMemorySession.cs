using System.Collections.Generic;
using Toto.WebApi.Caching.Core.Contracts;

namespace Toto.WebApi.Caching.Core.Core
{
    /// <summary>
    /// Represents an in memory instance of an <see cref="ISession"/> implementation.
    /// </summary>
    public class InMemorySession : ISession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemorySession" /> class.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public InMemorySession(string sessionId)
        {
            Dictionary = new Dictionary<string, object>();
            Id = sessionId;
        }

        /// <summary>
        /// Gets the dictionary which contains the items stored in the session.
        /// </summary>
        /// <value>
        /// The dictionary.
        /// </value>
        protected IDictionary<string, object> Dictionary { get; }

        /// <summary>
        /// Gets or sets the identifier for the session.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Adds an item with the specified name to the session.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="value">The item value.</param>
        public virtual void Add(string name, object value)
        {
            Dictionary.Add(name, value);
        }

        /// <summary>
        /// Removes an item with the specified name from the session.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        public virtual void Remove(string name)
        {
            Dictionary.Remove(name);
        }

        /// <summary>
        /// Retrieves an item with the specified name from the session.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name of the item.</param>
        /// <returns>The item specified by its name.</returns>
        public virtual TValue Get<TValue>(string name)
        {
            return (TValue)Dictionary[name];
        }
    }
}
