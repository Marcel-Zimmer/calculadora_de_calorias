
using CalculadoraCalorias.Api.BackgroundServices;
using CalculadoraCalorias.Api.Middlewares;
using CalculadoraCalorias.Application;
using CalculadoraCalorias.Core;
using CalculadoraCalorias.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()  
              .AllowAnyMethod(); 
    });
});

// 1. Serviços
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCore();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A String de Conexão 'DefaultConnection' não foi encontrada.");
}
 
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddHostedService<WorkerEstimativaIa>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

}
app.UseMiddleware<ExecaoMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("PermitirFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();