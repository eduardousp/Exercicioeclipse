using manage.aplication.commands;
using manage.aplication.query;
using MediatR;

namespace manage.ui.Endpoints
{
    public static class ProjectEndpoints
    {
        public static WebApplication AddProjectEndpoints(this WebApplication app)
        {
            app.MapGet("/users/{userId}/projects", async (int userId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProjectsByUserQuery(userId));
                return Results.Ok(result);
            }).WithName("GetProjectsByUser").WithOpenApi();//.RequireAuthorization(); 


            app.MapPost("/projects", async (int userId, CreateProjectCommand command, IMediator mediator) =>
            {
                var projectId = await mediator.Send(command);
                return Results.Created($"/projects/{projectId}", projectId);
            }).WithName("CreateProject").WithOpenApi();//.RequireAuthorization();

            app.MapDelete("/projects/{projectId}", async (int projectId, IMediator mediator) =>
                {
                    var result = await mediator.Send(new DeleteProjectCommand() { ProjectId= projectId });

                    if (!result)
                    {
                        return Results.BadRequest("Cannot delete project.");
                    }

                    return Results.Ok("Project deleted");
                })
                .WithName("DeleteProject")
                .WithOpenApi();

            return app;
        }
       

    }
}
