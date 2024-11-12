using manage.core.entities;

namespace manage.core.interfaces
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetJobsByProjectAsync(int projectId);
        Task<Job?> GetByIdAsync(int jobId);
        Task AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task DeleteAsync(Job job);
        Task<int> GetJobsCountByProjectAsync(int projectId);
    }
}
