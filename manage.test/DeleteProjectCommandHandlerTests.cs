using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using manage.aplication.commands;
using manage.aplication.handlers;
using manage.core.entities;
using manage.core.interfaces;
using System;

namespace manage.tests
{
    public class DeleteProjectCommandHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly DeleteProjectCommandHandler _handler;

        public DeleteProjectCommandHandlerTests()
        {

            _projectRepositoryMock = new Mock<IProjectRepository>();


            _handler = new DeleteProjectCommandHandler(_projectRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenProjectExists_ShouldDeleteProjectAndReturnTrue()
        {

            var deleteProjectCommand = new DeleteProjectCommand { ProjectId = 1 };

            var project = new Project
            {
                Id = deleteProjectCommand.ProjectId,
                Name = "Test Project"
            };

            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(deleteProjectCommand.ProjectId))
                                  .ReturnsAsync(project);

            _projectRepositoryMock.Setup(repo => repo.DeleteAsync(project))
                                  .Returns(Task.CompletedTask);


            var result = await _handler.Handle(deleteProjectCommand, CancellationToken.None);


            Assert.True(result); // Verifica se o projeto foi excluído com sucesso


            _projectRepositoryMock.Verify(repo => repo.GetByIdAsync(deleteProjectCommand.ProjectId), Times.Once);


            _projectRepositoryMock.Verify(repo => repo.DeleteAsync(project), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenProjectDoesNotExist_ShouldReturnFalse()
        {

            var deleteProjectCommand = new DeleteProjectCommand { ProjectId = 1 };


            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(deleteProjectCommand.ProjectId))
                                  .ReturnsAsync((Project)null);


            var result = await _handler.Handle(deleteProjectCommand, CancellationToken.None);


            Assert.False(result); // Verifica se o retorno é falso porque o projeto não foi encontrado


            _projectRepositoryMock.Verify(repo => repo.GetByIdAsync(deleteProjectCommand.ProjectId), Times.Once);


            _projectRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Project>()), Times.Never);
        }
    }
}