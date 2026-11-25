using Abstracciones.Animales;
using Abstracciones.Consultas;
using Abstracciones.Especies;
using Abstracciones.Razas;
using Abstracciones.Vacunas;
using Abstracciones.Veterinarias;
using DA;
using DA.Animales;
using DA.Consultas;
using DA.Contexto.AdopcionContexto;
using DA.Razas;
using DA.Seed;
using DA.Vacunas;
using DA.Veterinarias;
using Flujo.Animales;
using Flujo.Consultas;
using Flujo.Especies;
using Flujo.Razas;
using Flujo.VacunacionFlujo;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IEspecieDA, EspecieDA>();
builder.Services.AddScoped<IEspecieFlujo, EspecieFlujo>();
builder.Services.AddScoped<IRazaDA, RazaDA>();
builder.Services.AddScoped<IRazaFlujo, RazaFlujo>();
builder.Services.AddScoped<IAnimalDA, AnimalDA>();
builder.Services.AddScoped<IAnimalFlujo, AnimalFlujo>();
builder.Services.AddScoped<IVacunacionDA, VacunacionDA>();
builder.Services.AddScoped<IVacunacionFlujo, VacunacionFlujo>();
builder.Services.AddScoped<IVeterinariaDA, VeterinariaDA>();
builder.Services.AddScoped<IConsultaDA, ConsultaDA>();
builder.Services.AddScoped<IConsultaFlujo, ConsultaFlujo>();

var cadenaConexion = builder.Configuration.GetConnectionString("Adopcion")
    ?? throw new InvalidOperationException("Falta la cadena de conexión para Adopcion en la configuracion");

builder.Services.AddDbContext<AdopcionContexto>(opciones =>
    opciones.UseSqlServer(cadenaConexion));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await AdopcionSeeder.SeedAsync(app.Services);
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
