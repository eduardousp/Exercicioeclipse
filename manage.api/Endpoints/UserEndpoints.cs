using manage.aplication.dto;
using manage.aplication.query;
using manage.core.entities;
using manage.core.interfaces;
using MediatR;

namespace manage.ui.Endpoints
{
    public static class UserEndpoints
    {

        public static WebApplication AddUserEndpoints(this WebApplication app)
        {
            app.MapGet("/users/performance", async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetUserPerformanceReportQuery());
                    return Results.Ok(result);
                })
                .RequireAuthorization("Manager") // Middleware para checar a role
                .WithName("GetPerformanceReport")
                .WithOpenApi();
           
            app.MapPost("/register", async (UserDTO userDto, IUserRepository userRepository) =>
            {
                if (await userRepository.UserExistsAsync(userDto.Email))
                {
                    return Results.BadRequest("User already exists.");
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password); // Usar BCrypt para hash da senha

                var user = new User
                {
                    
                    Name = userDto.Name,
                    Email=userDto.Email,
                    Password = passwordHash
                };

                await userRepository.AddAsync(user);

                return Results.Ok("User registered successfully.");
            });
            app.MapPost("/login", async (LoginUserDto loginDto, IUserRepository userRepository, ITokenService tokenService) =>
            {
                var user = await userRepository.GetByUsernameAsync(loginDto.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                    return Results.Unauthorized();
                }

                var token = tokenService.GenerateToken(user);

                return Results.Ok(new { Token = token });
            });
            return app;
        }
    }
}
