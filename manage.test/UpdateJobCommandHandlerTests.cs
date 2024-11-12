using Moq;
using Xunit;
using manage.aplication.commands;
using manage.aplication.handlers;
using manage.core.entities;
using manage.core.interfaces;
using manage.aplication.dto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace manage.aplication.Tests
{
    public class UpdateJobCommandHandlerTests
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly UpdateJobCommandHandler _handler;

        public UpdateJobCommandHandlerTests()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _handler = new UpdateJobCommandHandler(_jobRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateJob_WhenValidRequest()
        {
            // Arrange
            var jobId = 1;
            var projectId = 1;
            var title = "Updated Job Title";
            var description = "Updated Job Description";
            var status = 1; // StatusJob enum value
            var comment = "Updated Job Comment";

            var command = new UpdateJobCommand
            {
                Id = jobId,
                ProjectId = projectId,
                Title = title,
                Description = description,
                Status = (StatusJob)status,
                Comment = comment
            };

            var existingJob = new Job
            {
                Id = jobId,
                Title = "Old Job Title",
                Description = "Old Job Description",
                Status = StatusJob.Concluida, // Assuming StatusJob is an enum
                Comment = "Old Job Comment"
            };

            // Simula a contagem de jobs
            _jobRepositoryMock.Setup(repo => repo.GetJobsCountByProjectAsync(projectId))
                .ReturnsAsync(5);

            // Simula o repositório retornando o job existente
            _jobRepositoryMock.Setup(repo => repo.GetByIdAsync(jobId))
                .ReturnsAsync(existingJob);

            // Simula a atualização bem-sucedida do job
            _jobRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Job>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(jobId, result.Id);
            Assert.Equal(title, result.Title);
            Assert.Equal(description, result.Description);
            Assert.Equal((StatusJob)status, result.Status);
            Assert.Equal(comment, result.Comment);

            // Verifica se o repositório de jobs foi chamado para atualizar o job
            _jobRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Job>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenJobNotFound()
        {
            // Arrange
            var jobId = 999;
            var projectId = 1;

            var command = new UpdateJobCommand
            {
                Id = jobId,
                ProjectId = projectId,
                Title = "Updated Title",
                Description = "Updated Description",
                Status = (StatusJob)1,
                Comment = "Updated Comment"
            };

            // Simula o repositório retornando null (job não encontrado)
            _jobRepositoryMock.Setup(repo => repo.GetByIdAsync(jobId))
                .ReturnsAsync((Job)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal($"Job com ID {jobId} não foi encontrado.", exception.Message,, true, CultureInfo.InvariantCulture);

            _jobRepositoryMock.Verify(repo => repo.GetByIdAsync(jobId), Times.Once);
            _jobRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Job>()), Times.Never); // Não deve chamar o update
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenTaskLimitReached()
        {
            
            var jobId = 1;
            var projectId = 1;

            var command = new UpdateJobCommand
            {
                Id = jobId,
                ProjectId = projectId,
                Title = "Updated Title",
                Description = "Updated Description",
                Status = (StatusJob)0,
                Comment = "Updated Comment"
            };

            // Simula a contagem de jobs atingindo o limite
            _jobRepositoryMock.Setup(repo => repo.GetJobsCountByProjectAsync(projectId))
                .ReturnsAsync(20); // Limite atingido

            // Simula o repositório retornando o job existente
            _jobRepositoryMock.Setup(repo => repo.GetByIdAsync(jobId))
                .ReturnsAsync(new Job { Id = jobId }); // Retorna o job existente

         
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Task limit reached for this project.", exception.Message);

            _jobRepositoryMock.Verify(repo => repo.GetJobsCountByProjectAsync(projectId), Times.Once);
            _jobRepositoryMock.Verify(repo => repo.GetByIdAsync(jobId), Times.Never);
        }
    }
}
