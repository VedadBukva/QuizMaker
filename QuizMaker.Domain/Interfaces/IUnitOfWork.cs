using System.Threading.Tasks;

namespace QuizMaker.Domain.Interfaces
{
    /// <summary>
    /// Defines a unit of work abstraction for coordinating data persistence.
    /// </summary>
    /// <remarks>
    /// Ensures that multiple repository operations are committed
    /// as a single transactional operation.
    /// 
    /// Typically implemented using Entity Framework DbContext.
    /// </remarks>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Persists all changes made within the current unit of work.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the underlying data store.
        /// </returns>
        /// <remarks>
        /// All tracked changes across repositories are committed in a single transaction.
        /// </remarks>
        Task<int> SaveChangesAsync();
    }
}
