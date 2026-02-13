using System.Web.Http;

namespace QuizMaker.Api
{
    /// <summary>
    /// Configures ASP.NET Web API routing, formatting and global services.
    /// </summary>
    /// <remarks>
    /// This configuration is executed during application startup and is responsible for:
    /// - Enabling attribute-based routing
    /// - Registering a default conventional route
    /// - Registering a global exception handler
    /// - Configuring JSON serialization settings (e.g., enums as strings)
    /// </remarks>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers Web API configuration for the application.
        /// </summary>
        /// <param name="config">HTTP configuration instance used to register routes and services.</param>
        /// <remarks>
        /// Routing:
        /// - Attribute routing is enabled to allow explicit route definitions on controllers/actions.
        /// - A fallback conventional route is registered for controllers using default conventions.
        /// 
        /// Error handling:
        /// - A global exception handler is registered to map application exceptions into consistent HTTP responses.
        /// 
        /// JSON:
        /// - Enums are serialized as strings (e.g., "Asc"/"Desc") instead of numeric values.
        /// </remarks>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Services.Replace(typeof(System.Web.Http.ExceptionHandling.IExceptionHandler), new Infrastructure.ApiExceptionHandler());

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new Newtonsoft.Json.Converters.StringEnumConverter()
            );
        }
    }
}
