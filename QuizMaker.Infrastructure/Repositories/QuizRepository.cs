using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Enums;
using QuizMaker.Domain.Interfaces;
using QuizMaker.Domain.Paging;
using QuizMaker.Infrastructure.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizMakerDbContext _context;

        /// <summary>
        /// QuizRepository constructor.
        /// </summary>
        /// <param name="context"></param>
        public QuizRepository(QuizMakerDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<PagedResult<Quiz>> GetPagedAsync(
            string search,
            int page,
            int pageSize,
            SortOrder sortOrder)
        {
            var query = _context.Quizzes
                .Include(q => q.QuizQuestions)
                .Where(q => !q.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(q => q.Name.Contains(search));
            }

            var totalCount = await query.CountAsync();

            query = sortOrder == SortOrder.Desc
                ? query.OrderByDescending(q => q.CreatedAtUtc)
                : query.OrderBy(q => q.CreatedAtUtc);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Quiz>(items, totalCount, page, pageSize);
        }

        /// <inheritdoc />
        public async Task<Quiz> GetByIdWithQuestionsAsync(Guid id)
        {
            return await _context.Quizzes
                .Include(q => q.QuizQuestions.Select(qq => qq.Question))
                .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);
        }

        /// <inheritdoc />
        public async Task AddAsync(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id)
        {
            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);

            if (quiz == null)
                return;

            quiz.IsDeleted = true;
            quiz.DeletedAtUtc = DateTime.UtcNow;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Quiz quiz)
        {
            _context.Entry(quiz).State = EntityState.Modified;
            await Task.CompletedTask;
        }
    }
}
