using manage.core.interfaces;
using MediatR;
using manage.aplication.commands;

namespace manage.aplication.handlers
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IProjectRepository _projectRepository;

        public CreateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new core.entities.Project
            {
                UserId = request.UserId,
                Name = request.Name,
                Description = request.Description
            };

            await _projectRepository.AddAsync(project);
            return project.Id;
        }


    }

}
