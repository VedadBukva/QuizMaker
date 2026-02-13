using Autofac;
using Autofac.Integration.WebApi;
using QuizMaker.Api;
using QuizMaker.Application.Services;
using QuizMaker.Domain.Export;
using QuizMaker.Domain.Interfaces;
using QuizMaker.Infrastructure.Data;
using QuizMaker.Infrastructure.Export;
using QuizMaker.Infrastructure.Repositories;
using System;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Http;

namespace QuizMaker
{
    /// <summary>
    /// Global configuration
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Application Start configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            var config = GlobalConfiguration.Configuration;

            WebApiConfig.Register(config);

            var exportersPath = HostingEnvironment.MapPath("~/Exporters");
            if (string.IsNullOrWhiteSpace(exportersPath))
                exportersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exporters");

            Directory.CreateDirectory(exportersPath);

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<QuizMakerDbContext>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<QuizRepository>()
                .As<IQuizRepository>()
                .InstancePerRequest();

            builder.RegisterType<QuestionRepository>()
                .As<IQuestionRepository>()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder.RegisterType<QuizService>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<QuestionService>()
                .AsSelf()
                .InstancePerRequest();

            builder.Register(c => new MefExporterCatalog(exportersPath))
                .As<IExporterCatalog>()
                .SingleInstance();

            builder.RegisterType<ExporterService>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<QuizExportService>()
                .AsSelf()
                .InstancePerRequest();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.EnsureInitialized();
        }
    }
}