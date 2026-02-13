using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Enums;
using QuizMaker.Domain.Paging;
using System;
using System.Threading.Tasks;

namespace QuizMaker.Domain.Interfaces
{
    /// <summary>
    /// Defines data access operations for <see cref="Quiz"/> entities.
    /// </summary>
    /// <remarks>
    /// Provides querying and persistence operations for quizzes, including:
    /// - Retrieving paged quiz lists (without loading question details)
    /// - Retrieving a single quiz with its associated questions
    /// - Creating, updating, and deleting quizzes
    /// 
    /// Questions are associated with quizzes through the <see cref="QuizQuestion"/> join entity.
    /// </remarks>
    public interface IQuizRepository
    {
        /// <summary>
        /// Retrieves a paginated list of quizzes.
        /// </summary>
        /// <param name="search">
        /// Optional text used to filter quizzes by name.
        /// </param>
        /// <param name="page">
        /// Page number (starting from 1).
        /// </param>
        /// <param name="pageSize">
        /// Number of items per page.
        /// </param>
        /// <param name="sortOrder">
        /// Sort direction applied to the results.
        /// </param>
        /// <returns>
        /// A paged result containing matching quizzes.
        /// </returns>
        /// <remarks>
        /// This method is intended for list views and should avoid loading full question details
        /// for performance reasons.
        /// </remarks>
        Task<PagedResult<Quiz>> GetPagedAsync(
            string search,
            int page,
            int pageSize,
            SortOrder sortOrder);

        /// <summary>
        /// Retrieves a quiz by its identifier including associated questions.
        /// </summary>
        /// <param name="id">Quiz identifier.</param>
        /// <returns>
        /// A quiz including its associated questions if found; otherwise null.
        /// </returns>
        /// <remarks>
        /// Intended for quiz detail view and export operations where full question data is required.
        /// </remarks>
        Task<Quiz> GetByIdWithQuestionsAsync(Guid id);

        /// <summary>
        /// Adds a new quiz to the data store.
        /// </summary>
        /// <param name="quiz">Quiz to add.</param>
        Task AddAsync(Quiz quiz);

        /// <summary>
        /// Deletes a quiz identified by the specified identifier.
        /// </summary>
        /// <param name="id">Quiz identifier.</param>
        /// <remarks>
        /// Deleting a quiz should not delete the associated questions, only the quiz entity itself
        /// (typically via soft delete).
        /// </remarks>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Updates an existing quiz in the data store.
        /// </summary>
        /// <param name="quiz">Quiz entity with updated values.</param>
        Task UpdateAsync(Quiz quiz);
    }
}