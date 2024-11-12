using manage.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.core.interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }

}
