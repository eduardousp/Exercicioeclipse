using manage.aplication.dto;
using manage.core.entities;
using MediatR;

namespace manage.aplication.commands
{
    public class CreateJobCommand : IRequest<JobDTO>
    {
        public int JobId { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public PriorityJob Priority { get; set; } // Baixa, Média, Alta

        public StatusJob Status { get; set; }
       
    }
}