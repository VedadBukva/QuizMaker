using QuizMaker.Application.Dtos;
using QuizMaker.Application.Exceptions;
using QuizMaker.Application.Requests;
using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Enums;
using QuizMaker.Domain.Interfaces;
using QuizMaker.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Application.Services
{
    /// <summary>
    /// Service responsible for managing quiz lifecycle operations.
    /// </summary>
    /// <remarks>
    /// Provides functionality for creating, updating, retrieving, deleting, 
    /// and paginating quizzes. 
    /// 
    /// Handles business rules such as:
    /// - Question recycling
    /// - Display order management
    /// - Soft deletion
    /// - Mapping domain entities to DTOs
    /// </remarks>
    public class QuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizService"/> class.
        /// </summary>
        /// <param name="quizRepository">Repository for quiz data access.</param>
        /// <param name="questionRepository">Repository for question data access.</param>
        /// <param name="unitOfWork">Unit of work used to persist changes.</param>
        public QuizService(
            IQuizRepository quizRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves paginated list of quizzes.
        /// </summary>
        /// <param name="request">Search, paging and sorting parameters.</param>
        /// <returns>
        /// A paged result containing lightweight quiz representations.
        /// </returns>
        /// <remarks>
        /// Supports:
        /// - Text-based search by quiz name
        /// - Pagination
        /// - Sorting by creation date
        /// </remarks>
        public async Task<PagedResult<QuizListItemDto>> GetPagedAsync(QuizPagedRequest request)
        {
            var page = request?.Page ?? 1;
            var pageSize = request?.PageSize ?? 50;
            var search = request?.Search;
            var sortOrder = request?.SortOrder ?? SortOrder.Desc;

            var pagedResult = await _quizRepository.GetPagedAsync(search, page, pageSize, sortOrder);

            var totalCount = pagedResult.TotalCount;

            var dtos = pagedResult.Items
                .Select(q => new QuizListItemDto
                {
                    Id = q.Id,
                    Name = q.Name,
                    QuestionCount = q.QuizQuestions?.Count ?? 0,
                    CreatedAtUtc = q.CreatedAtUtc
                })
                .ToList();

            return new PagedResult<QuizListItemDto>(dtos, totalCount, page, pageSize);
        }

        /// <summary>
        /// Retrieves detailed information about a specific quiz.
        /// </summary>
        /// <param name="id">Unique identifier of the quiz.</param>
        /// <returns>
        /// Detailed quiz information including ordered questions.
        /// Returns null if the quiz does not exist.
        /// </returns>
        /// <remarks>
        /// Questions are returned in their defined display order.
        /// </remarks>
        public async Task<QuizDetailsDto> GetByIdAsync(Guid id)
        {
            var quiz = await _quizRepository.GetByIdWithQuestionsAsync(id);
            if (quiz == null)
                throw new EntityNotFoundException(nameof(quiz), id.ToString());

            var questions = (quiz.QuizQuestions ?? new List<QuizQuestion>())
                .OrderBy(qq => qq.DisplayOrder)
                .Select(qq => new QuizQuestionDto
                {
                    Id = qq.QuestionId,
                    Text = qq.Question?.Text,
                    CorrectAnswer = qq.Question?.CorrectAnswer
                })
                .ToList();

            return new QuizDetailsDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                Questions = questions
            };
        }

        /// <summary>
        /// Creates a new quiz.
        /// </summary>
        /// <param name="request">
        /// Data required to create the quiz, including:
        /// - Quiz name
        /// - Existing question identifiers
        /// - Newly created questions
        /// </param>
        /// <returns>The unique identifier of the newly created quiz.</returns>
        /// <exception cref="MissingArgumentException">
        /// Thrown when the request model is null.
        /// </exception>
        /// <remarks>
        /// Supports question recycling by linking existing questions.
        /// Newly created questions are persisted before associating them with the quiz.
        /// Display order is automatically assigned based on insertion order.
        /// </remarks>
        public async Task<Guid> CreateAsync(CreateQuizRequest request)
        {
            if (request == null)
                throw new MissingArgumentException("Request model for Create Quiz is missing.", nameof(request));

            var now = DateTime.UtcNow;

            var existingIds = (request.ExistingQuestionIds ?? new List<Guid>())
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            var existingQuestions = await _questionRepository.GetByIdsAsync(existingIds);
            var existingById = existingQuestions.ToDictionary(q => q.Id, q => q);

            var newQuestions = (request.NewQuestions ?? new List<NewQuestionRequest>())
                .Where(q => q != null && !string.IsNullOrWhiteSpace(q.Text) && !string.IsNullOrWhiteSpace(q.CorrectAnswer))
                .Select(q => new Question
                {
                    Id = Guid.NewGuid(),
                    Text = q.Text.Trim(),
                    CorrectAnswer = q.CorrectAnswer.Trim(),
                    CreatedAtUtc = now,
                    UpdatedAtUtc = null
                })
                .ToList();

            if (newQuestions.Count > 0)
                await _questionRepository.AddRangeAsync(newQuestions);

            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Name = (request.Name ?? string.Empty).Trim(),
                IsDeleted = false,
                DeletedAtUtc = null,
                CreatedAtUtc = now,
                UpdatedAtUtc = null,
                QuizQuestions = new List<QuizQuestion>()
            };

            var sort = 0;

            foreach (var id in existingIds)
            {
                if (!existingById.ContainsKey(id))
                    continue;

                quiz.QuizQuestions.Add(new QuizQuestion
                {
                    Quiz = quiz,
                    QuestionId = id,
                    DisplayOrder = sort++,
                    CreatedAtUtc = now
                });
            }

            foreach (var nq in newQuestions)
            {
                quiz.QuizQuestions.Add(new QuizQuestion
                {
                    Quiz = quiz,
                    Question = nq,
                    DisplayOrder = sort++,
                    CreatedAtUtc = now
                });
            }

            await _quizRepository.AddAsync(quiz);
            await _unitOfWork.SaveChangesAsync();

            return quiz.Id;
        }

        /// <summary>
        /// Updates an existing quiz.
        /// </summary>
        /// <param name="id">Identifier of the quiz to update.</param>
        /// <param name="request">
        /// Updated quiz data including:
        /// - New quiz name
        /// - Existing question identifiers
        /// - Newly created questions
        /// </param>
        /// <returns>
        /// True if the update was successful.
        /// </returns>
        /// <exception cref="MissingArgumentException">
        /// Thrown when the request model is null.
        /// </exception>
        /// <exception cref="EntityNotFoundException">
        /// Thrown when the quiz does not exist.
        /// </exception>
        /// <remarks>
        /// Existing quiz-question relationships are cleared and rebuilt.
        /// Questions remain stored in the system even if removed from the quiz.
        /// </remarks>
        public async Task<bool> UpdateAsync(Guid id, UpdateQuizRequest request)
        {
            if (request == null)
                throw new MissingArgumentException("Request model for Update Quiz is missing.", nameof(request));

            var quiz = await _quizRepository.GetByIdWithQuestionsAsync(id);
            if (quiz == null)
                throw new EntityNotFoundException(nameof(quiz), id.ToString());

            var now = DateTime.UtcNow;

            quiz.Name = (request.Name ?? string.Empty).Trim();
            quiz.UpdatedAtUtc = now;

            var existingIds = (request.ExistingQuestionIds ?? new List<Guid>())
                .Where(qid => qid != Guid.Empty)
                .Distinct()
                .ToList();

            var existingQuestions = await _questionRepository.GetByIdsAsync(existingIds);
            var existingById = existingQuestions.ToDictionary(q => q.Id, q => q);

            var newQuestions = (request.NewQuestions ?? new List<NewQuestionRequest>())
                .Where(q => q != null && !string.IsNullOrWhiteSpace(q.Text) && !string.IsNullOrWhiteSpace(q.CorrectAnswer))
                .Select(q => new Question
                {
                    Text = q.Text.Trim(),
                    CorrectAnswer = q.CorrectAnswer.Trim(),
                    CreatedAtUtc = now,
                    UpdatedAtUtc = null
                })
                .ToList();

            if (newQuestions.Count > 0)
                await _questionRepository.AddRangeAsync(newQuestions);

            quiz.QuizQuestions = quiz.QuizQuestions ?? new List<QuizQuestion>();
            quiz.QuizQuestions.Clear();

            var sort = 0;

            foreach (var qid in existingIds)
            {
                if (!existingById.ContainsKey(qid))
                    continue;

                quiz.QuizQuestions.Add(new QuizQuestion
                {
                    QuizId = quiz.Id,
                    QuestionId = qid,
                    DisplayOrder = sort++,
                    CreatedAtUtc = now
                });
            }

            foreach (var nq in newQuestions)
            {
                quiz.QuizQuestions.Add(new QuizQuestion
                {
                    QuizId = quiz.Id,
                    Question = nq,
                    DisplayOrder = sort++,
                    CreatedAtUtc = now
                });
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a quiz.
        /// </summary>
        /// <param name="id">Identifier of the quiz to delete.</param>
        /// <returns>True if deletion was successful.</returns>
        /// <exception cref="EntityNotFoundException">
        /// Thrown when the quiz does not exist.
        /// </exception>
        /// <remarks>
        /// Deleting a quiz does not remove associated questions from the system.
        /// </remarks>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var quiz = await _quizRepository.GetByIdWithQuestionsAsync(id);
            if (quiz == null)
                throw new EntityNotFoundException(nameof(quiz), id.ToString());

            await _quizRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
