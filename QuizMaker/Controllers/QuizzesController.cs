using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using QuizMaker.Application.Requests;
using QuizMaker.Application.Services;

namespace QuizMaker.Api.Controllers
{
    /// <summary>
    /// Provides CRUD operations for managing quizzes.
    /// </summary>
    /// <remarks>
    /// Supports:
    /// - Retrieving paginated quiz list
    /// - Retrieving quiz details
    /// - Creating quizzes (with question recycling support)
    /// - Updating quizzes
    /// - Deleting quizzes
    /// 
    /// Quizzes consist of reusable questions.
    /// </remarks>
    [RoutePrefix("api/quizzes")]
    public class QuizzesController : ApiController
    {
        private readonly QuizService _quizService;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizzesController"/> class.
        /// </summary>
        /// <param name="quizService">
        /// Service responsible for quiz-related business logic.
        /// </param>
        public QuizzesController(QuizService quizService)
        {
            _quizService = quizService;
        }

        /// <summary>
        /// Retrieves a paginated list of quizzes.
        /// </summary>
        /// <param name="request">
        /// Search, paging and sorting parameters.
        /// </param>
        /// <returns>
        /// A paged result containing quiz summaries.
        /// </returns>
        /// <response code="200">Returns paginated quizzes.</response>
        /// <response code="400">Invalid request parameters.</response>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetPaged([FromUri] QuizPagedRequest request)
        {
            var result = await _quizService.GetPagedAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves detailed information about a specific quiz.
        /// </summary>
        /// <param name="id">Quiz identifier.</param>
        /// <returns>
        /// Quiz details including associated questions.
        /// </returns>
        /// <response code="200">Quiz found.</response>
        /// <response code="404">Quiz not found.</response>
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            var quiz = await _quizService.GetByIdAsync(id);
            return Ok(quiz);
        }

        /// <summary>
        /// Creates a new quiz.
        /// </summary>
        /// <param name="request">
        /// Quiz creation data including name, existing question IDs and new questions.
        /// </param>
        /// <returns>
        /// The identifier of the newly created quiz.
        /// </returns>
        /// <response code="201">Quiz created successfully.</response>
        /// <response code="400">Request body is missing or invalid.</response>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create([FromBody] CreateQuizRequest request)
        {
            var id = await _quizService.CreateAsync(request);
            return Created($"/api/quizzes/{id}", new { id });
        }

        /// <summary>
        /// Updates an existing quiz.
        /// </summary>
        /// <param name="id">Identifier of the quiz to update.</param>
        /// <param name="request">Updated quiz data.</param>
        /// <returns>
        /// No content if update is successful.
        /// </returns>
        /// <response code="204">Quiz updated successfully.</response>
        /// <response code="400">Request body is missing or invalid.</response>
        /// <response code="404">Quiz not found.</response>
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Update(Guid id, [FromBody] UpdateQuizRequest request)
        {
            await _quizService.UpdateAsync(id, request);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes a quiz.
        /// </summary>
        /// <param name="id">Identifier of the quiz to delete.</param>
        /// <returns>
        /// No content if deletion is successful.
        /// </returns>
        /// <response code="204">Quiz deleted successfully.</response>
        /// <response code="404">Quiz not found.</response>
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await _quizService.DeleteAsync(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }

}
