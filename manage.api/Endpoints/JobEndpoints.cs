using manage.aplication.query;
using MediatR;
using manage.aplication.commands;

namespace manage.ui.Endpoints
{
    public static class JobEndpoints
    {
        public static WebApplication AddJobEndpoints(this WebApplication app)
        {
            app.MapGet("/projects/{projectId}/jobs", async (int projectId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetJobsByProjectQuery(projectId));
                return Results.Ok(result);
            }).WithName("GetJobsByProject")
            .WithOpenApi();

            app.MapPost("/projects/{projectId}/jobs", async (int projectId, CreateJobCommand command, IMediator mediator) =>
                {                      
                    command.ProjectId = projectId;
                    var result = await mediator.Send(command);
                    return Results.Created($"/projects/{projectId}/jobs/{command.JobId}", result);
                   
                })
                .WithName("CreateJob")
                .WithOpenApi();
            app.MapPut("/jobs/{jobId}", async (int jobId, UpdateJobCommand command, IMediator mediator) =>
                {
                    command.Id = jobId;
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                })
                .WithName("UpdateJob")
                .WithOpenApi();

            app.MapDelete("/jobs/{jobId}", async (int jobId, IMediator mediator) =>
                {
                    var result = await mediator.Send(new DeleteJobCommand(jobId));
                    if (!result)
                    {
                        return Results.BadRequest("Cannot delete job.");
                    }

                    return Results.Ok("Job deleted");
                })
                .WithName("DeleteJob")
                .WithOpenApi();
            app.MapPost("/jobs/{jobId}/comments", async (int jobId, AddJobCommentCommand command, IMediator mediator) =>
                {
                    command.JobId = jobId;
                    var result = await mediator.Send(command);


                    return Results.Ok(result);
                })
                .WithName("AddJobComment")
                .WithOpenApi();
            return app;
        }

        
    }
}
