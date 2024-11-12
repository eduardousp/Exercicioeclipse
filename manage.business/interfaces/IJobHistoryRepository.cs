using manage.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.core.interfaces
{
    public interface IJobHistoryRepository
    {
        Task<JobHistory?> GetByIdAsync(int id);
        Task<IEnumerable<JobHistory>> GetAllAsync();
        Task AddAsync(JobHistory jobHistory);
      
    }
}
