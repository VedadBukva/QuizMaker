using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Interfaces;
using QuizMaker.Domain.Paging;
using QuizMaker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuizMakerDbContext _context;

        /// <summary>
        /// QuestionRepository contrusctor.
        /// </summary>
        /// <param name="context"></param>
        public QuestionRepository(QuizMakerDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<PagedResult<Question>> SearchPagedAsync(
            string search,
            int page,
            int pageSize)
        {
            IQueryable<Question> query = _context.Questions.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(q => q.Text.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(q => q.CreatedAtUtc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Question>(items, totalCount, page, pageSize);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Question>> GetByIdsAsync(IReadOnlyCollection<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return Array.Empty<Question>();

            var items = await _context.Questions
                .Where(q => ids.Contains(q.Id))
                .ToListAsync();

            return items;
        }

        /// <inheritdoc />
        public async Task AddRangeAsync(IEnumerable<Question> questions)
        {
            var list = questions?.ToList() ?? new List<Question>();
            if (list.Count == 0)
                return;

            _context.Questions.AddRange(list);
            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task AddAsync(Question question)
        {
            _context.Questions.Add(question);
            await Task.CompletedTask;
        }
    }
}
