using manage.aplication.dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.aplication.query
{
    public record GetProjectsByUserQuery(int UserId) : IRequest<IEnumerable<dto.ProjectDTO>>;
}
