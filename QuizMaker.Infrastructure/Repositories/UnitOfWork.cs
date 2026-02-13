using System.Threading.Tasks;
using QuizMaker.Domain.Interfaces;
using QuizMaker.Infrastructure.Data;

namespace QuizMaker.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuizMakerDbContext _context;

        /// <summary>
        /// UnitOfWork constructor.
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(QuizMakerDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
