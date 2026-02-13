using System;

namespace QuizMaker.Domain.Entities.Helpers
{
    /// <summary>
    /// Base class for all domain entities.
    /// </summary>
    /// <remarks>
    /// Provides common audit and lifecycle properties shared across entities,
    /// including soft deletion and timestamp tracking.
    /// </remarks>
    public class BaseEntity
    {
        /// <summary>
        /// Indicates whether the entity has been soft-deleted.
        /// </summary>
        /// <remarks>
        /// Soft-deleted entities remain stored in the database
        /// but should be excluded from active queries.
        /// </remarks>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// UTC date and time when the entity was soft-deleted.
        /// </summary>
        /// <remarks>
        /// Null if the entity has not been deleted.
        /// </remarks>
        public DateTime? DeletedAtUtc { get; set; }

        /// <summary>
        /// UTC date and time when the entity was created.
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>
        /// UTC date and time when the entity was last updated.
        /// </summary>
        /// <remarks>
        /// Null if the entity has never been updated.
        /// </remarks>
        public DateTime? UpdatedAtUtc { get; set; }
    }

}
