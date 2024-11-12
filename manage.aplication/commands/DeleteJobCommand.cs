using manage.aplication.dto;
using manage.core.entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.aplication.commands
{
    public class DeleteJobCommand : IRequest<bool>
    {
        public DeleteJobCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PriorityJob Priority { get; set; } // Baixa, Média, Alta

        public StatusJob Status { get; set; }
        public string Comment { get; set; }
    }
}
