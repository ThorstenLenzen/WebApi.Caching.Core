namespace Toto.WebApi.Caching.Core.Contracts
{
    /// <summary>
    /// Represents a session identified by its id.
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Gets or sets the identifier for the session.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Adds an item with the specified name to the session.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="value">The item value.</param>
        void Add(string name, object value);

        /// <summary>
        /// Removes an item with the specified name from the session.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        void Remove(string name);

        /// <summary>
        /// Retrieves an item with the specified name from the session.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="name">The name of the item.</param>
        /// <returns>The item specified by its name.</returns>
        TValue Get<TValue>(string name);
    }
}
