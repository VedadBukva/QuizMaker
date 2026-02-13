using QuizMaker.Application.Dtos;
using QuizMaker.Application.Exceptions;
using QuizMaker.Application.Requests;
using QuizMaker.Application.Validation;
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
            var page = request?.Page ?? ValidationConstants.DefaultPage;
            var pageSize = request?.PageSize ?? ValidationConstants.DefaultPageSize;
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
                throw new MissingArgumentException("Request body is required.", nameof(request));

            var now = DateTime.UtcNow;

            var quizName = NormalizeRequired(request.Name, "Quiz name is required.", nameof(request.Name), ValidationConstants.QuizNameMaxLength);

            var existingIds = NormalizeIds(request.ExistingQuestionIds);
            var newQuestions = NormalizeNewQuestions(request.NewQuestions, now);

            EnsureHasAtLeastOneQuestion(existingIds, newQuestions);

            var existingById = await LoadExistingQuestionsOrThrow(existingIds);

            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Name = quizName,
                IsDeleted = false,
                DeletedAtUtc = null,
                CreatedAtUtc = now,
                UpdatedAtUtc = null,
                QuizQuestions = new List<QuizQuestion>()
            };

            if (newQuestions.Count > 0)
                await _questionRepository.AddRangeAsync(newQuestions);

            quiz.QuizQuestions = BuildQuizQuestions(quiz.Id, quiz, existingIds, existingById, newQuestions, now);

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
            if (id == Guid.Empty)
                throw new MissingArgumentException("Quiz id is required.", nameof(id));

            if (request == null)
                throw new MissingArgumentException("Request body is required.", nameof(request));

            var quiz = await _quizRepository.GetByIdWithQuestionsAsync(id);
            if (quiz == null)
                throw new EntityNotFoundException(nameof(Quiz), id.ToString());

            var now = DateTime.UtcNow;

            var quizName = NormalizeRequired(request.Name, "Quiz name is required.", nameof(request.Name), ValidationConstants.QuizNameMaxLength);
            var existingIds = NormalizeIds(request.ExistingQuestionIds);
            var newQuestions = NormalizeNewQuestions(request.NewQuestions, now);

            EnsureHasAtLeastOneQuestion(existingIds, newQuestions);

            var existingById = await LoadExistingQuestionsOrThrow(existingIds);

            quiz.Name = quizName;
            quiz.UpdatedAtUtc = now;

            if (newQuestions.Count > 0)
                await _questionRepository.AddRangeAsync(newQuestions);

            quiz.QuizQuestions = quiz.QuizQuestions ?? new List<QuizQuestion>();
            quiz.QuizQuestions.Clear();

            var built = BuildQuizQuestions(quiz.Id, quiz, existingIds, existingById, newQuestions, now);
            foreach (var qq in built)
                quiz.QuizQuestions.Add(qq);

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
            if (id == Guid.Empty)
                throw new MissingArgumentException("Quiz id is required.", nameof(id));

            var quiz = await _quizRepository.GetByIdWithQuestionsAsync(id);
            if (quiz == null)
                throw new EntityNotFoundException(nameof(quiz), id.ToString());

            await _quizRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static string NormalizeRequired(string value, string message, string paramName, int maxLen)
        {
            var v = (value ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(v))
                throw new MissingArgumentException(message, paramName);

            if (v.Length > maxLen)
                throw new MissingArgumentException($"'{paramName}' exceeds maximum length of {maxLen}.", paramName);

            return v;
        }

        private static List<Guid> NormalizeIds(IList<Guid> ids)
        {
            return (ids ?? new List<Guid>())
                .Where(x => x != Guid.Empty)
                .Distinct()
                .ToList();
        }

        private List<Question> NormalizeNewQuestions(IList<NewQuestionRequest> reqs, DateTime now)
        {
            var normalized = (reqs ?? new List<NewQuestionRequest>())
                .Where(q => q != null)
                .Select(q => new
                {
                    Text = (q.Text ?? string.Empty).Trim(),
                    Answer = (q.CorrectAnswer ?? string.Empty).Trim()
                })
                .Where(q => !string.IsNullOrWhiteSpace(q.Text) && !string.IsNullOrWhiteSpace(q.Answer))
                .ToList();

            if (normalized.Any(q => q.Text.Length > ValidationConstants.QuestionTextMaxLength))
                throw new MissingArgumentException($"Question text exceeds maximum length of {ValidationConstants.QuestionTextMaxLength}.", nameof(NewQuestionRequest.Text));

            if (normalized.Any(q => q.Answer.Length > ValidationConstants.CorrectAnswerMaxLength))
                throw new MissingArgumentException($"Correct answer exceeds maximum length of {ValidationConstants.CorrectAnswerMaxLength}.", nameof(NewQuestionRequest.CorrectAnswer));

            var dup = normalized
                .GroupBy(x => x.Text, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);

            if (dup != null)
                throw new MissingArgumentException("Duplicate new questions are not allowed within the same request.", nameof(CreateQuizRequest.NewQuestions));

            return normalized
                .Select(q => new Question
                {
                    Id = Guid.NewGuid(),
                    Text = q.Text,
                    CorrectAnswer = q.Answer,
                    CreatedAtUtc = now,
                    UpdatedAtUtc = null,
                    IsDeleted = false,
                    DeletedAtUtc = null
                })
                .ToList();
        }

        private static void EnsureHasAtLeastOneQuestion(IReadOnlyCollection<Guid> existingIds, IReadOnlyCollection<Question> newQuestions)
        {
            if ((existingIds == null || existingIds.Count == 0) && (newQuestions == null || newQuestions.Count == 0))
                throw new MissingArgumentException("Quiz must contain at least one question.", "questions");
        }

        private async Task<Dictionary<Guid, Question>> LoadExistingQuestionsOrThrow(IReadOnlyCollection<Guid> existingIds)
        {
            if (existingIds == null || existingIds.Count == 0)
                return new Dictionary<Guid, Question>();

            var existingQuestions = await _questionRepository.GetByIdsAsync(existingIds);
            var existingById = existingQuestions.ToDictionary(q => q.Id, q => q);

            var missing = existingIds.Where(id => !existingById.ContainsKey(id)).ToList();
            if (missing.Count > 0)
                throw new MissingArgumentException("One or more existingQuestionIds do not exist.", nameof(CreateQuizRequest.ExistingQuestionIds));

            return existingById;
        }

        private static List<QuizQuestion> BuildQuizQuestions(
            Guid quizId,
            Quiz quiz,
            IReadOnlyList<Guid> existingIds,
            IDictionary<Guid, Question> existingById,
            IReadOnlyList<Question> newQuestions,
            DateTime now)
        {
            var result = new List<QuizQuestion>();
            var order = 0;

            foreach (var id in existingIds ?? new List<Guid>())
            {
                if (existingById != null && existingById.ContainsKey(id))
                {
                    result.Add(new QuizQuestion
                    {
                        QuizId = quizId,
                        Quiz = quiz,
                        QuestionId = id,
                        DisplayOrder = order++,
                        CreatedAtUtc = now,
                        IsDeleted = false,
                        DeletedAtUtc = null
                    });
                }
            }

            foreach (var nq in newQuestions ?? new List<Question>())
            {
                result.Add(new QuizQuestion
                {
                    QuizId = quizId,
                    Quiz = quiz,
                    QuestionId = nq.Id,
                    Question = nq,
                    DisplayOrder = order++,
                    CreatedAtUtc = now,
                    IsDeleted = false,
                    DeletedAtUtc = null
                });
            }

            return result;
        }
    }
}
