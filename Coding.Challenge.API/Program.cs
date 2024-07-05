using Coding.Challenge.API.Extensions;
using Coding.Challenge.API.Repositories;
using Coding.Challenge.Dependencies.Database;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args)
        .ConfigureWebHost()
        .RegisterServices();


var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connection, ServerVersion.AutoDetect(connection)));



builder.Services.AddScoped(typeof(IDatabase<,>), typeof(DbContextAdapter<,>));

var app = builder.Build();

// Configuração do Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger";
});

// Mapeamento dos controllers
app.MapControllers();

app.Run();