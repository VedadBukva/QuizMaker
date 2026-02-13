using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuizMaker.Application.Exceptions;
using QuizMaker.Application.Requests;
using QuizMaker.Application.Services;
using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Export;
using QuizMaker.Domain.Interfaces;
using QuizMaker.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[TestClass]
public class QuizExportServiceTests
{
    [TestMethod]
    public async Task CreateAsync_ShouldCreateQuiz_AndReturnId()
    {
        var quizRepoMock = new Mock<IQuizRepository>();
        var questionRepoMock = new Mock<IQuestionRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        var service = new QuizService(
            quizRepoMock.Object,
            questionRepoMock.Object,
            unitOfWorkMock.Object);

        var request = new CreateQuizRequest
        {
            Name = "Test Quiz",
            ExistingQuestionIds = new List<Guid>(),
            NewQuestions = new List<NewQuestionRequest>
        {
            new NewQuestionRequest
            {
                Text = "Pitanje 1",
                CorrectAnswer = "Odgovor"
            }
        }
        };

        var id = await service.CreateAsync(request);

        Assert.AreNotEqual(Guid.Empty, id);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task SearchPagedAsync_ShouldReturnPagedResult()
    {
        var repoMock = new Mock<IQuestionRepository>();

        var questions = new List<Question>
    {
        new Question
        {
            Id = Guid.NewGuid(),
            Text = "Test",
            CreatedAtUtc = DateTime.UtcNow
        }
    };

        repoMock
            .Setup(r => r.SearchPagedAsync(null, 1, 50))
            .ReturnsAsync(new PagedResult<Question>(questions, 1, 1, 50));

        var service = new QuestionService(repoMock.Object);

        var result = await service.SearchPagedAsync(new QuestionPagedRequest());

        Assert.AreEqual(1, result.TotalCount);
        Assert.HasCount(1, result.Items);
    }

    [TestMethod]
    public async Task ExportAsync_ShouldThrow_WhenExporterDoesNotExist()
    {
        var quizRepoMock = new Mock<IQuizRepository>();
        var catalogMock = new Mock<IExporterCatalog>();

        catalogMock
            .Setup(c => c.GetByKey("invalid"))
            .Returns((IQuizExporter)null);

        var service = new QuizExportService(
            quizRepoMock.Object,
            catalogMock.Object);

        await Assert.ThrowsExactlyAsync<ExporterNotFoundException>(() =>
        service.ExportAsync(Guid.NewGuid(), "invalid"));
    }
}
