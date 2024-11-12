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
using manage.aplication.query;

public class GetProjectsByUserQueryHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly GetProjectsByUserQueryHandler _handler;

    public GetProjectsByUserQueryHandlerTests()
    {
        // Mock do repositório de projetos
        _projectRepositoryMock = new Mock<IProjectRepository>();

        // Instancia o handler passando o repositório simulado
        _handler = new GetProjectsByUserQueryHandler(_projectRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfProjectDTOs_WhenProjectsExist()
    {
        // Arrange
        var userId = 1;
        var projects = new List<Project>
        {
            new Project { Id = 1, Name = "Project 1", Description = "Description 1", UserId = userId },
            new Project { Id = 2, Name = "Project 2", Description = "Description 2", UserId = userId }
        };

        var query = new GetProjectsByUserQuery(userId);

        // Simula o repositório retornando uma lista de projetos
        _projectRepositoryMock.Setup(repo => repo.GetProjectsByUserAsync(userId))
                              .ReturnsAsync(projects);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result); // Verifica que o resultado não é nulo
        Assert.Equal(2, result.Count()); // Verifica que o número de projetos retornado está correto

        var projectList = result.ToList();

        Assert.Equal(projects[0].Id, projectList[0].Id);
        Assert.Equal(projects[0].Name, projectList[0].Name);
        Assert.Equal(projects[0].Description, projectList[0].Description);

        Assert.Equal(projects[1].Id, projectList[1].Id);
        Assert.Equal(projects[1].Name, projectList[1].Name);
        Assert.Equal(projects[1].Description, projectList[1].Description);

        // Verifica que foi chamado com o ID correto
        _projectRepositoryMock.Verify(repo => repo.GetProjectsByUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        var userId = 1;
        var query = new GetProjectsByUserQuery(userId);

        // Simula o repositório retornando uma lista vazia de projetos
        _projectRepositoryMock.Setup(repo => repo.GetProjectsByUserAsync(userId))
                              .ReturnsAsync(new List<Project>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result); 
        Assert.Empty(result); 

        // Verifica que  foi chamado com o ID correto
        _projectRepositoryMock.Verify(repo => repo.GetProjectsByUserAsync(userId), Times.Once);
    }
}
