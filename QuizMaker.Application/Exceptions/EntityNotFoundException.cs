using System;

namespace QuizMaker.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested entity cannot be found in the system.
    /// </summary>
    /// <remarks>
    /// This exception is typically used in the application layer when an entity
    /// with a specific identifier does not exist in the data store.
    /// It is commonly mapped to an HTTP 404 (Not Found) response.
    /// </remarks>
    public sealed class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entity">Name of the entity type.</param>
        /// <param name="key">Identifier value used to search for the entity.</param>
        public EntityNotFoundException(string entity, string key)
            : base($"Entity {entity} with ID {key} not found.")
        {
            Entity = entity;
            Key = key;
        }

        /// <summary>
        /// Gets the name of the entity that was not found.
        /// </summary>
        public string Entity { get; }

        /// <summary>
        /// Gets the identifier value used in the lookup.
        /// </summary>
        public string Key { get; }
    }

}
