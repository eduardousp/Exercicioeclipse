namespace manage.ui.Endpoints
{
    public static class EndpointsExtentions
    {
        public static WebApplication MapEndpoints(this WebApplication app)
        {
            app.AddUserEndpoints()
                .AddProjectEndpoints()
                .AddJobEndpoints();
              
            return app;
        }
    }
}
