using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using manage.aplication.commands;
using manage.aplication.handlers;
using manage.core.entities;
using manage.core.interfaces;
using System;

namespace manage.aplication.Tests
{
    public class UpdateCommentJobCommandHandlerTests
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IJobHistoryRepository> _jobHistoryRepositoryMock;
        private readonly UpdateCommentJobCommandHandler _handler;

        public UpdateCommentJobCommandHandlerTests()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _jobHistoryRepositoryMock = new Mock<IJobHistoryRepository>();
            _handler = new UpdateCommentJobCommandHandler(_jobRepositoryMock.Object, _jobHistoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateCommentAndAddToHistory_WhenJobExists()
        {
            // Arrange
            var jobId = 1;
            var userId = 1;
            var newComment = "This is a new comment";

            var existingJob = new Job
            {
                Id = jobId,
                Comment = "Existing comment"
            };

            var command = new AddJobCommentCommand
            {
                JobId = jobId,
                Comment = newComment,
                UserId = userId
            };

            // Simula o repositório retornando o job existente
            _jobRepositoryMock.Setup(repo => repo.GetByIdAsync(jobId))
                .ReturnsAsync(existingJob);

            // Simula o sucesso na atualização do job
            _jobRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Job>()))
                .Returns(Task.CompletedTask);

            // Simula o sucesso na adição ao histórico
            _jobHistoryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<JobHistory>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal($"New comment added: Existing comment/**/This is a new comment", result);
            _jobRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Job>()), Times.Once);
            _jobHistoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<JobHistory>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenJobDoesNotExist()
        {
            // Arrange
            var jobId = 999;
            var userId = 1;
            var newComment = "This is a new comment";

            var command = new AddJobCommentCommand
            {
                JobId = jobId,
                Comment = newComment,
                UserId = userId
            };

            // Simula o repositório retornando null (job não encontrado)
            _jobRepositoryMock.Setup(repo => repo.GetByIdAsync(jobId))
                .ReturnsAsync((Job)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal($"Job com ID {jobId} não foi encontrado.", exception.Message,StringComparer.InvariantCulture);
            _jobRepositoryMock.Verify(repo => repo.GetByIdAsync(jobId), Times.Once);
            _jobRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Job>()), Times.Never); // Verifica que a atualização não foi chamada
        }

        [Fact]
        public async Task Handle_ShouldUpdateComment_WhenCommentIsEmpty()
        {
            // Arrange
            var jobId = 1;
            var userId = 1;
            var existingComment = "Existing comment";

            var command = new AddJobCommentCommand
            {
                JobId = jobId,
                Comment = "", // Comentário vazio
                UserId = userId
            };

            var existingJob = new Job
            {
                Id = jobId,
                Comment = existingComment
            };

            // Simula o repositório retornando o job existente
            _jobRepositoryMock.Setup(repo => repo.GetByIdAsync(jobId))
                .ReturnsAsync(existingJob);

            // Simula o sucesso na atualização do job
            _jobRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Job>()))
                .Returns(Task.CompletedTask);

            // Simula o sucesso na adição ao histórico
            _jobHistoryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<JobHistory>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal($"New comment added: Existing comment", result);
            _jobRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Job>()), Times.Once);
            _jobHistoryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<JobHistory>()), Times.Once);
        }
    }
}
