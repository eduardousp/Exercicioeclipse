using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using manage.aplication.dto;
using manage.aplication.handlers;
using manage.core.entities;
using manage.core.interfaces;
using manage.aplication.commands;
using manage.aplication.query; // Ajuste para usar a namespace correta para a query

namespace manage.tests
{
    public class GetJobsByProjectQueryHandlerTests
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly GetJobsByProjectQueryHandler _handler;

        public GetJobsByProjectQueryHandlerTests()
        {
            // Mock do repositório de jobs
            _jobRepositoryMock = new Mock<IJobRepository>();

            // Instancia o handler passando o repositório simulado
            _handler = new GetJobsByProjectQueryHandler(_jobRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfJobDTOs_WhenJobsExist()
        {
            // Arrange
            var projectId = 1;
            var jobs = new List<Job>
        {
            new Job { Id = 1, Title = "Job 1", Description = "Description 1", Comment = "Comment 1", Priority = PriorityJob.Alta, Status = StatusJob.EmAndamento },
            new Job { Id = 2, Title = "Job 2", Description = "Description 2", Comment = "Comment 2", Priority = PriorityJob.Baixa, Status = StatusJob.Concluida }
        };

            var query = new GetJobsByProjectQuery(projectId);

            // Simula o repositório retornando uma lista de jobs
            _jobRepositoryMock.Setup(repo => repo.GetJobsByProjectAsync(projectId))
                              .ReturnsAsync(jobs);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result); // Verifica que o resultado não é nulo
            Assert.Equal(2, result.Count()); // Verifica que o número de jobs retornado está correto

            var jobList = result.ToList();

            Assert.Equal(jobs[0].Id, jobList[0].Id);
            Assert.Equal(jobs[0].Title, jobList[0].Title);
            Assert.Equal(jobs[0].Description, jobList[0].Description);

            Assert.Equal(jobs[1].Id, jobList[1].Id);
            Assert.Equal(jobs[1].Title, jobList[1].Title);
            Assert.Equal(jobs[1].Description, jobList[1].Description);

            // Verifica que o GetJobsByProjectAsync foi chamado com o ID correto
            _jobRepositoryMock.Verify(repo => repo.GetJobsByProjectAsync(projectId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoJobsExist()
        {

            var projectId = 1;
            var query = new GetJobsByProjectQuery(projectId);


            _jobRepositoryMock.Setup(repo => repo.GetJobsByProjectAsync(projectId)).ReturnsAsync(new List<Job>());


            var result = await _handler.Handle(query, CancellationToken.None);


            Assert.NotNull(result);
            Assert.Empty(result);


            _jobRepositoryMock.Verify(repo => repo.GetJobsByProjectAsync(projectId), Times.Once);
        }
    }
}