using manage.ui.Configs;
using manage.ui.Endpoints;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.AddServices().Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


//// Habilitar o middleware SwaggerUI para navegação
//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
//    options.RoutePrefix = string.Empty;  // Deixa o Swagger acessível na raiz da aplicação
//});
var supportedCultures = new[] { new CultureInfo("pt-BR") };

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);



app.UseHttpsRedirection();
app.MapEndpoints();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
