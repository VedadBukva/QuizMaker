using QuizMaker.Application.Dtos;
using QuizMaker.Application.Exceptions;
using QuizMaker.Application.Requests;
using QuizMaker.Application.Validation;
using QuizMaker.Domain.Interfaces;
using QuizMaker.Domain.Paging;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaker.Application.Services
{
    /// <summary>
    /// Service responsible for managing question-related operations.
    /// </summary>
    /// <remarks>
    /// Provides functionality for searching and retrieving questions
    /// with pagination support. Acts as an application layer abstraction
    /// over the question repository.
    /// </remarks>
    public class QuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionService"/> class.
        /// </summary>
        /// <param name="questionRepository">
        /// Repository used for accessing question data.
        /// </param>
        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        /// <summary>
        /// Searches questions using optional text filtering and pagination.
        /// </summary>
        /// <param name="request">
        /// Search and pagination parameters.
        /// </param>
        /// <returns>
        /// A paged result containing lightweight question representations.
        /// </returns>
        /// <remarks>
        /// If no search term is provided, all questions are returned in a paged format.
        /// Pagination defaults to page 1 and page size 50 if not specified.
        /// </remarks>
        public async Task<PagedResult<QuestionListItemDto>> SearchPagedAsync(QuestionPagedRequest request)
        {
            var page = request?.Page ?? ValidationConstants.DefaultPage;
            var pageSize = request?.PageSize ?? ValidationConstants.DefaultPageSize;
            var search = request?.Search;

            var pagedResult = await _questionRepository.SearchPagedAsync(search, page, pageSize);

            var totalCount = pagedResult.TotalCount;

            var dtos = pagedResult.Items
                .Select(q => new QuestionListItemDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    CreatedAtUtc = q.CreatedAtUtc
                })
                .ToList();

            return new PagedResult<QuestionListItemDto>(dtos, totalCount, page, pageSize);
        }
    }
}
