using manage.core.entities;


namespace manage.core.interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsByUserAsync(int userId);
        Task<Project?> GetByIdAsync(int projectId);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
    }
}
