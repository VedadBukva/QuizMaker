using Swashbuckle.Application;
using System.Web.Http;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(QuizMaker.Api.App_Start.SwaggerConfig), "Register")]

namespace QuizMaker.Api.App_Start
{
    /// <summary>
    /// Configures Swagger (OpenAPI) documentation for the Web API.
    /// </summary>
    /// <remarks>
    /// Swagger is used to provide interactive API documentation via Swagger UI.
    /// 
    /// This configuration:
    /// - Registers Swagger endpoints
    /// - Defines API version metadata
    /// - Includes XML documentation files to enrich endpoint and model descriptions
    /// 
    /// XML documentation files must be enabled in project build settings
    /// (Project Properties -> Build -> XML documentation file).
    /// </remarks>
    public class SwaggerConfig
    {
        /// <summary>
        /// Registers Swagger and Swagger UI for the application.
        /// </summary>
        /// <remarks>
        /// The method loads XML documentation files (if present) from the application's bin folder
        /// to enrich Swagger descriptions for controllers, actions, and DTOs.
        /// </remarks>
        public static void Register()
        {
            var apiXmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "bin\\QuizMaker.Api.xml";
            var applicationXmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "bin\\QuizMaker.Application.xml";
            var domainXmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "bin\\QuizMaker.Domain.xml";

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "QuizMaker API");
                    if (System.IO.File.Exists(apiXmlPath))
                    {
                        c.IncludeXmlComments(apiXmlPath);
                    }
                    if (System.IO.File.Exists(applicationXmlPath))
                    {
                        c.IncludeXmlComments(applicationXmlPath);
                    }
                    if (System.IO.File.Exists(domainXmlPath))
                    {
                        c.IncludeXmlComments(domainXmlPath);
                    }
                })
                .EnableSwaggerUi();

            
        }
    }
}
