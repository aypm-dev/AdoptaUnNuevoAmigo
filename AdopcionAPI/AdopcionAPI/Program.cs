using Abstracciones.Especies;
using AdopcionAPI.Errores;
using DA;
using DA.Contexto.AdopcionContexto;
using Flujo.Especies;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Errores estándar (RFC 9457, application/problem+json) y manejador global de excepciones de negocio.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorExcepcionesGlobal>();

builder.Services.AddScoped<IEspecieDA, EspecieDA>();
builder.Services.AddScoped<IEspecieFlujo, EspecieFlujo>();

var cadenaConexion = builder.Configuration.GetConnectionString("Adopcion")
    ?? throw new InvalidOperationException("Falta la cadena de conexión para Adopcion en la configuracion");

builder.Services.AddDbContext<AdopcionContexto>(opciones =>
    opciones.UseSqlServer(cadenaConexion));

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
