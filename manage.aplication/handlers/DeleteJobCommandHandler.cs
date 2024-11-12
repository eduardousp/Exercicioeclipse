using manage.aplication.commands;
using manage.core.interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand, bool>
{
    private readonly IJobRepository _jobRepository;

    // Injetar dependências, como o repositório de projetos
    public DeleteJobCommandHandler(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    // Implementação do método Handle que processa a exclusão
    public async Task<bool> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verifica se o projeto existe
            var job = await _jobRepository.GetByIdAsync(request.Id);
            if (job == null)
            {
                throw new Exception($"Job com ID {request.Id} não foi encontrado.");
            }

            // Lógica de exclusão
            await _jobRepository.DeleteAsync(job);

            // Retorna Unit para indicar que a operação foi concluída
            return true;
        }
        catch (Exception)
        {

            return false;
        }
     
    }

   
}

