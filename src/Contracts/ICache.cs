namespace Toto.WebApi.Caching.Core.Contracts
{
    /// <summary>
    /// Represents a cache implementation for the PlanIt system.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets the or creates the session for the specified id, if it not already exists.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>A <see cref="ISession"/>An instance for the specified id.</returns>
        ISession GetOrCreateSession(string sessionId);

        /// <summary>
        /// Adds an item with the specified name to the cache.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="value">The item value.</param>
        void Add(string name, object value);

        /// <summary>
        /// Removes an item with the specified name from the cache.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        void Remove(string name);

        /// <summary>
        /// Retrieves an item with the specified name from the cache.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name of the item.</param>
        /// <returns>The item specified by its name.</returns>
        TValue Get<TValue>(string name);
    }
}
