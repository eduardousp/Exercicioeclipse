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
    public class AddJobCommentCommand : IRequest<string>
    {
        public int JobId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
    }
}
