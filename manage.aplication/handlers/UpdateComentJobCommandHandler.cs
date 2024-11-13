using manage.aplication.commands;
using manage.aplication.dto;
using manage.core.entities;
using manage.core.interfaces;
using MediatR;

namespace manage.aplication.handlers;

public class UpdateCommentJobCommandHandler : IRequestHandler<AddJobCommentCommand, string>
{
    private readonly IJobRepository _jobRepository;
    private readonly IJobHistoryRepository _jobHistoryRepository;

    public UpdateCommentJobCommandHandler(IJobRepository jobRepository, IJobHistoryRepository jobHistoryRepository)
    {
        _jobRepository = jobRepository;
        _jobHistoryRepository = jobHistoryRepository;
    }

    public async Task<string> Handle(AddJobCommentCommand request, CancellationToken cancellationToken)
    {
        // Adicionar comentário ao histórico
       
        try
        {
            var job= await _jobRepository.GetByIdAsync(request.JobId);

          

           
            if (job == null)
            {
                throw new Exception($"Job com ID {request.JobId} nao foi encontrado.");
            }

         
            job.Comment = !string.IsNullOrWhiteSpace(request.Comment) ? job.Comment+"/**/"+request.Comment : job.Comment;
            
       

            await _jobRepository.UpdateAsync(job);
            await _jobHistoryRepository.AddAsync( new JobHistory
            {
                JobId = job.Id,
                ChangeDescription = $"New comment added: {job.Comment}",
                UserId = request.UserId,
                ChangeDate=DateTime.Now
            });
            return $"New comment added: {job.Comment}";
    }
        catch (Exception ex)
        {

            return ex.ToString();
        }
      
    }
}
