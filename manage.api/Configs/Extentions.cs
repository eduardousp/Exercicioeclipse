using manage.aplication.commands;
using manage.aplication.dto;
using manage.aplication.handlers;
using manage.aplication.query;
using manage.core.entities;
using manage.core.interfaces;
using manage.infra.context;
using manage.infra.data.Repositories;
using manage.infra.data.Service;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace manage.ui.Configs
{
    public static class Extentions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<Context>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IJobHistoryRepository, JobHistoryRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddTransient<IRequestHandler<CreateProjectCommand, int>, CreateProjectCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<aplication.query.GetProjectsByUserQuery, IEnumerable<aplication.dto.ProjectDTO>>, GetProjectsByUserQueryHandler>();
            builder.Services.AddTransient<IRequestHandler<DeleteProjectCommand, bool>, DeleteProjectCommandHandler>();
            builder.Services.AddTransient <IRequestHandler<aplication.query.GetJobsByProjectQuery, IEnumerable<aplication.dto.JobDTO >>, GetJobsByProjectQueryHandler>();
            builder.Services.AddTransient<IRequestHandler<CreateJobCommand, JobDTO>, CreateJobCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<UpdateJobCommand, JobDTO>, UpdateJobCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<DeleteJobCommand, bool>, DeleteJobCommandHandler>();
            builder.Services.AddTransient<IRequestHandler<AddJobCommentCommand, string>, UpdateCommentJobCommandHandler>();


            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"]
                     //   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            builder.Services.AddAuthorization();
            var lista = new List<string>();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no campo abaixo: \nExemplo: Bearer {seu token}"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
           return builder;
        }
    }
}
