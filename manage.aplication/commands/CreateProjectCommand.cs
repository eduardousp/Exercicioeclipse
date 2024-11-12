using MediatR;


namespace manage.aplication.commands
{
    public record CreateProjectCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public string Name { get; init; }
        public string Description { get; init; }

    }
}
