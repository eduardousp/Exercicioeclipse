using manage.aplication.commands;
using manage.aplication.dto;
using manage.core.entities;
using manage.core.interfaces;
using MediatR;

namespace manage.aplication.handlers;

public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, JobDTO>
{
    private readonly IJobRepository _jobRepository;

    public UpdateJobCommandHandler(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobDTO> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var jobCount = await _jobRepository.GetJobsCountByProjectAsync(request.ProjectId);

            if (jobCount >= 20)
            {
                throw new Exception("Task limit reached for this project.");
            }

            var job = await _jobRepository.GetByIdAsync(request.Id);
            if (job == null)
            {
                throw new Exception($"Job com ID {request.Id} não foi encontrado.");
            }

            job.Title = !string.IsNullOrWhiteSpace(request.Title) ? request.Title : job.Title;
            job.Description = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : job.Description;
            job.Status = (StatusJob)request.Status;
            job.Comment = !string.IsNullOrWhiteSpace(request.Comment) ? request.Comment : job.Comment;
            
       

            await _jobRepository.UpdateAsync(job);

            return new JobDTO
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Status = job.Status,
                Comment= job.Comment

            };
        }
        catch (Exception )
        {

            throw;
        }
      
    }
}
