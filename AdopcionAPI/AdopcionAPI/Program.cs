using System.Text.Json.Serialization;
using Abstracciones.Comunes;
using AdopcionAPI.Errores;
using DA;
using Flujo;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Errores estándar (RFC 9457, application/problem+json) y manejador global de excepciones de negocio.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorExcepcionesGlobal>();

// Registro modular de dependencias en capas
builder.Services.AddDatos(builder.Configuration);
builder.Services.AddFlujo();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Endpoint de salud (200 si la base contesta, 503 si no)
app.MapGet("/salud", async (ISaludBaseDatos salud, CancellationToken ct) =>
        await salud.EstaDisponibleAsync(ct)
            ? Results.Ok(new { estado = "ok", baseDeDatos = "disponible" })
            : Results.Json(new { estado = "degradado", baseDeDatos = "no disponible" },
                statusCode: StatusCodes.Status503ServiceUnavailable))
    .WithTags("Operación")
    .AllowAnonymous();

app.UseAuthorization();

app.MapControllers();

app.Run();
