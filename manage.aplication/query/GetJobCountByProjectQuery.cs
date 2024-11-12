using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.aplication.query
{
    public record GetJobCountByProjectQuery(int ProjectId) : IRequest<int>;
}
