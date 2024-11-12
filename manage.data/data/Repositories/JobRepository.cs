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
    public class JobRepository : IJobRepository
    {
        private readonly Context _context;

        public JobRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Job>> GetJobsByProjectAsync(int projectId)
        {
            return await _context.Jobs
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Job?> GetByIdAsync(int jobId)
        {
            return await _context.Jobs
                .FirstOrDefaultAsync(t => t.Id == jobId);
        }

        public async Task AddAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Job job)
        {
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetJobsCountByProjectAsync(int projectId)
        {
            return  _context.Jobs
              .Where(t => t.ProjectId == projectId).Count();
        }
    }

}
