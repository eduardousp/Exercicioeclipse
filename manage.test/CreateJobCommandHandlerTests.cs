using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using manage.aplication.commands;
using manage.aplication.dto;
using manage.aplication.handlers;
using manage.core.entities;
using manage.core.interfaces;
using System;
using System.Timers;

public class CreateJobCommandHandlerTests
{
    private readonly Mock<IJobRepository> _jobRepositoryMock;
    private readonly CreateJobCommandHandler _handler;

    public CreateJobCommandHandlerTests()
    {
     
        _jobRepositoryMock = new Mock<IJobRepository>();

     
        _handler = new CreateJobCommandHandler(_jobRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenJobCountIsLessThanLimit_ShouldCreateJob()
    {
       
        var createJobCommand = new CreateJobCommand
        {
            ProjectId = 1,
            Title = "Test Job",
            Description = "Job Description",
            Comment = "Some Comment",
            Priority = (PriorityJob)1, 
            Status = 0    
        };

       
        _jobRepositoryMock.Setup(repo => repo.GetJobsCountByProjectAsync(createJobCommand.ProjectId))
                          .ReturnsAsync(5); 

        
        _jobRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Job>()))
                          .Returns(Task.CompletedTask);

        var result = await _handler.Handle(createJobCommand, CancellationToken.None);

      
        Assert.NotNull(result);
        Assert.Equal(createJobCommand.Title, result.Title);
        Assert.Equal(createJobCommand.Description, result.Description);
        Assert.Equal(createJobCommand.Comment, result.Comment);
        Assert.Equal((PriorityJob)createJobCommand.Priority, result.Priority);
        Assert.Equal((StatusJob)createJobCommand.Status, result.Status);

       
        _jobRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Job>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenJobCountIsAtLimit_ShouldThrowException()
    {
        // Arrange
        var createJobCommand = new CreateJobCommand
        {
            ProjectId = 1,
            Title = "Test Job",
            Description = "Job Description",
            Comment = "Some Comment",
            Priority = (PriorityJob)1, 
            Status = 0   
        };

      
        _jobRepositoryMock.Setup(repo => repo.GetJobsCountByProjectAsync(createJobCommand.ProjectId))
                          .ReturnsAsync(20); // Exatamente o limite

      
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(createJobCommand, CancellationToken.None));

       
        Assert.Equal("Task limit reached for this project.", exception.Message);

     
        _jobRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Job>()), Times.Never);
    }
}
