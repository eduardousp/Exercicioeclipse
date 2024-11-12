using manage.core.entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.aplication.commands
{
    public class DeleteProjectCommand : IRequest<bool>
    {
       public int ProjectId { get; set; }
      
    }
}
