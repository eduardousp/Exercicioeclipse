using manage.core.entities;
using manage.core.interfaces;
using manage.infra.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.infra.data.Repositories
{
    public class JobHistoryRepository : IJobHistoryRepository
    {
        private readonly Context _context;
        public JobHistoryRepository(Context context)
        {
            _context = context;
        }
        public async Task AddAsync(JobHistory jobHistory)
        {
            await _context.JobHistory.AddAsync(jobHistory);
            await _context.SaveChangesAsync();
        }

       

        public async Task<IEnumerable<JobHistory>> GetAllAsync()
        {
            return await _context.JobHistory.ToListAsync();
        }

        public async Task<JobHistory?> GetByIdAsync(int id)
        {
            return await _context.JobHistory
                .FirstOrDefaultAsync(t => t.Id == id);

        }

      
    }
}
