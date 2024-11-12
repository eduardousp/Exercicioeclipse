using manage.aplication.commands;
using manage.core.interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    // Injetar dependências, como o repositório de projetos
    public DeleteProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    // Implementação do método Handle que processa a exclusão
    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verifica se o projeto existe
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new Exception($"Projeto com ID {request.ProjectId} não foi encontrado.");
            }

            // Lógica de exclusão
            await _projectRepository.DeleteAsync(project);

            // Retorna Unit para indicar que a operação foi concluída
            return true;
        }
        catch (Exception)
        {

            return false;
        }
     
    }

   
}

