using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using QuizMaker.Application.Exceptions;

namespace QuizMaker.Api.Infrastructure
{
    /// <summary>
    /// Global exception handler for the Web API.
    /// </summary>
    /// <remarks>
    /// Intercepts unhandled exceptions thrown during request processing
    /// and converts them into consistent HTTP responses.
    /// 
    /// Maps:
    /// - <see cref="EntityNotFoundException"/> to 404 (Not Found)
    /// - <see cref="ExporterNotFoundException"/> to 404 (Not Found)
    /// - <see cref="MissingArgumentException"/> to 400 (Bad Request)
    /// - All other exceptions to 500 (Internal Server Error)
    /// 
    /// Ensures that internal exception details are not exposed to clients.
    /// </remarks>
    public sealed class ApiExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Handles unhandled exceptions and transforms them into HTTP responses.
        /// </summary>
        /// <param name="context">
        /// Context containing the thrown exception and HTTP request details.
        /// </param>
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context == null) return;

            var ex = context.Exception;

            if (ex is EntityNotFoundException)
            {
                context.Result = Error(context, HttpStatusCode.NotFound, ex.Message);
                return;
            }

            if (ex is ExporterNotFoundException)
            {
                context.Result = Error(context, HttpStatusCode.NotFound, ex.Message);
                return;
            }

            if (ex is MissingArgumentException)
            {
                context.Result = Error(context, HttpStatusCode.BadRequest, ex.Message);
                return;
            }

            context.Result = Error(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }

        /// <summary>
        /// Creates a standardized error response.
        /// </summary>
        /// <param name="ctx">Exception handling context.</param>
        /// <param name="code">HTTP status code to return.</param>
        /// <param name="message">Error message to include in response body.</param>
        /// <returns>An HTTP action result containing error payload.</returns>
        private static IHttpActionResult Error(ExceptionHandlerContext ctx, HttpStatusCode code, string message)
        {
            var payload = new
            {
                Message = message
            };

            return new ResponseMessageResult(ctx.Request.CreateResponse(code, payload));
        }
    }
}
