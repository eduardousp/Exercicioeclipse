using manage.core.interfaces;
using MediatR;

namespace manage.aplication.handlers
{
    public class GetProjectsByUserQueryHandler : IRequestHandler<query.GetProjectsByUserQuery, IEnumerable<dto.ProjectDTO>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectsByUserQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<dto.ProjectDTO>> Handle(query.GetProjectsByUserQuery request, CancellationToken cancellationToken)
        {
            var projetos = await _projectRepository.GetProjectsByUserAsync(request.UserId);

            var projetosDTO = new List<dto.ProjectDTO>();
            foreach (var project in projetos) { 
                projetosDTO.Add(new dto.ProjectDTO() 
                {
                    Id=project.Id,
                    Description=project.Description,
                    Name=project.Name
                });
            }

            return projetosDTO;
        }
    }

}
