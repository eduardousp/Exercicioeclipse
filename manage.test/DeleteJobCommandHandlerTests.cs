using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using manage.aplication.commands;
using manage.aplication.handlers;
using manage.core.entities;
using manage.core.interfaces;
using System;

public class DeleteJobCommandHandlerTests
{
    private readonly Mock<IJobRepository> _jobRepositoryMock;
    private readonly DeleteJobCommandHandler _handler;

    public DeleteJobCommandHandlerTests()
    {
     
        _jobRepositoryMock = new Mock<IJobRepository>();

     
        _handler = new DeleteJobCommandHandler(_jobRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenJobExists_ShouldDeleteJobAndReturnTrue()
    {
   
        var deleteJobCommand = new DeleteJobCommand(1);

        var job = new Job
        {
            Id = deleteJobCommand.Id,
            Title = "Test Job"
        };


        _jobRepositoryMock.Setup(repo => repo.GetByIdAsync(deleteJobCommand.Id))
                          .ReturnsAsync(job);


        _jobRepositoryMock.Setup(repo => repo.DeleteAsync(job))
                          .Returns(Task.CompletedTask);

    
        var result = await _handler.Handle(deleteJobCommand, CancellationToken.None);

      
        Assert.True(result); 


        _jobRepositoryMock.Verify(repo => repo.GetByIdAsync(deleteJobCommand.Id), Times.Once);

     
        _jobRepositoryMock.Verify(repo => repo.DeleteAsync(job), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenJobDoesNotExist_ShouldReturnFalse()
    {
     
        var deleteJobCommand = new DeleteJobCommand(1);

    
        _jobRepositoryMock.Setup(repo => repo.GetByIdAsync(deleteJobCommand.Id))
                          .ReturnsAsync((Job)null);

     
        var result = await _handler.Handle(deleteJobCommand, CancellationToken.None);

        Assert.False(result); // Verifica se o retorno é falso porque o job não foi encontrado

        // Verifica se GetByIdAsync foi chamado com o ID correto
        _jobRepositoryMock.Verify(repo => repo.GetByIdAsync(deleteJobCommand.Id), Times.Once);

        // Verifica que DeleteAsync não foi chamado
        _jobRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Job>()), Times.Never);
    }
}
