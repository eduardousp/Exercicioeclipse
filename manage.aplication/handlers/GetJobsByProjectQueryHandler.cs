using manage.aplication.dto;
using manage.core.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace manage.aplication.handlers
{
    public class GetJobsByProjectQueryHandler : IRequestHandler<query.GetJobsByProjectQuery, IEnumerable<JobDTO>>
    {
        private readonly IJobRepository _jobRepository;

        public GetJobsByProjectQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<IEnumerable<JobDTO>> Handle(query.GetJobsByProjectQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _jobRepository.GetJobsByProjectAsync(request.ProjectId);
            var jobsDTO=new List<JobDTO>();
            foreach (var job in jobs)
            {
                jobsDTO.Add(new JobDTO{ 
                        Id = job.Id,
                        Description = job.Description,
                        Comment = job.Comment,
                        Priority   =job.Priority,
                        Status = job.Status,
                        Title=job.Title
                      });
            }
            return jobsDTO;
        }
    }
}
