using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using manage.aplication.commands;
using manage.aplication.handlers;
using manage.core.entities;
using manage.core.interfaces;

namespace manage.tests
{
    public class CreateProjectCommandHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly CreateProjectCommandHandler _handler;

        public CreateProjectCommandHandlerTests()
        {

            _projectRepositoryMock = new Mock<IProjectRepository>();


            _handler = new CreateProjectCommandHandler(_projectRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateProjectAndReturnProjectId()
        {

            var createProjectCommand = new CreateProjectCommand
            {
                UserId = 1,
                Name = "Test Project",
                Description = "Project Description"
            };

            var project = new Project
            {
                Id = 10,
                UserId = createProjectCommand.UserId,
                Name = createProjectCommand.Name,
                Description = createProjectCommand.Description
            };

            // Simula a adição de um projeto e que o ID foi gerado
            _projectRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Project>()))
                                  .Callback<Project>(p => p.Id = project.Id) // Simula a geração do ID
                                  .Returns(Task.CompletedTask);

            var result = await _handler.Handle(createProjectCommand, CancellationToken.None);

            // Assert
            Assert.Equal(project.Id, result); // Verifica se o ID retornado é o esperado

            // Verifica se o AddAsync foi chamado com um projeto que tem os mesmos valores do request
            _projectRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Project>(
                p => p.UserId == createProjectCommand.UserId &&
                     p.Name == createProjectCommand.Name &&
                     p.Description == createProjectCommand.Description)),
                Times.Once);
        }
    }
}