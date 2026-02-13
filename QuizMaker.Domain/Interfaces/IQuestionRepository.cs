using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizMaker.Domain.Interfaces
{
    /// <summary>
    /// Defines data access operations for <see cref="Question"/> entities.
    /// </summary>
    /// <remarks>
    /// Provides querying and persistence operations for quiz questions.
    /// Typically used when creating or updating quizzes,
    /// especially for question recycling scenarios.
    /// </remarks>
    public interface IQuestionRepository
    {
        /// <summary>
        /// Searches questions with pagination support.
        /// </summary>
        /// <param name="search">
        /// Optional text used to filter questions by their content.
        /// </param>
        /// <param name="page">
        /// Page number (starting from 1).
        /// </param>
        /// <param name="pageSize">
        /// Number of items per page.
        /// </param>
        /// <returns>
        /// A paged result containing matching questions.
        /// </returns>
        Task<PagedResult<Question>> SearchPagedAsync(
            string search,
            int page,
            int pageSize);

        /// <summary>
        /// Retrieves questions by their identifiers.
        /// </summary>
        /// <param name="ids">
        /// Collection of question identifiers.
        /// </param>
        /// <returns>
        /// A collection of matching questions.
        /// </returns>
        Task<IEnumerable<Question>> GetByIdsAsync(IReadOnlyCollection<Guid> ids);

        /// <summary>
        /// Adds multiple questions to the data store.
        /// </summary>
        /// <param name="questions">
        /// Collection of questions to add.
        /// </param>
        Task AddRangeAsync(IEnumerable<Question> questions);

        /// <summary>
        /// Adds a single question to the data store.
        /// </summary>
        /// <param name="question">
        /// Question to add.
        /// </param>
        Task AddAsync(Question question);
    }
}
