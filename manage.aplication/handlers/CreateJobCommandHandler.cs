using manage.aplication.commands;
using manage.aplication.dto;
using manage.core.entities;
using manage.core.interfaces;
using MediatR;

namespace manage.aplication.handlers;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, JobDTO>
{
    private readonly IJobRepository _jobRepository;

    public CreateJobCommandHandler(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<JobDTO> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var jobCount = await _jobRepository.GetJobsCountByProjectAsync(request.ProjectId);

        if (jobCount >= 20)
        {
            throw new Exception("Task limit reached for this project.");
        }

        var job = new Job
        {

            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            Comment = request.Comment,
            Priority = (PriorityJob)request.Priority,
            Status = (StatusJob)request.Status
        };

        await _jobRepository.AddAsync(job);

        return new JobDTO
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Status = (StatusJob)job.Status,
            Comment = job.Comment,
            Priority = (PriorityJob)job.Priority,
        };
    }
}
