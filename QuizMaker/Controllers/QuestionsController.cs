using System.Threading.Tasks;
using System.Web.Http;
using QuizMaker.Application.Requests;
using QuizMaker.Application.Services;

namespace QuizMaker.Api.Controllers
{
    /// <summary>
    /// Provides operations related to quiz questions.
    /// </summary>
    /// <remarks>
    /// Supports searching and retrieving questions with pagination.
    /// Typically used when selecting existing questions while creating or updating a quiz.
    /// </remarks>
    [RoutePrefix("api/questions")]
    public class QuestionsController : ApiController
    {
        private readonly QuestionService _questionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionsController"/> class.
        /// </summary>
        /// <param name="questionService">
        /// Service responsible for question-related operations.
        /// </param>
        public QuestionsController(QuestionService questionService)
        {
            _questionService = questionService;
        }

        /// <summary>
        /// Retrieves a paginated list of questions.
        /// </summary>
        /// <param name="request">
        /// Optional search and pagination parameters.
        /// </param>
        /// <returns>
        /// A paged result containing lightweight question representations.
        /// </returns>
        /// <response code="200">Returns paginated questions.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <remarks>
        /// If no search term is provided, all questions are returned in paged format.
        /// Results are ordered by creation date (descending).
        /// </remarks>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetPaged([FromUri] QuestionPagedRequest request)
        {
            var result = await _questionService.SearchPagedAsync(request);
            return Ok(result);
        }
    }
}
