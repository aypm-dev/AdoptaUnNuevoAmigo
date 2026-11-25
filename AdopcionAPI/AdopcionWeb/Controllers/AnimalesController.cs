using AdopcionWeb.Clients;
using InertiaCore;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Consultas.ConsultaDtos;
using static Abstracciones.Vacunas.VacunacionDtos;

namespace AdopcionWeb.Controllers;

public class AnimalesController : Controller
{
    private readonly AnimalesApiClient _animalesApi;

    public AnimalesController(AnimalesApiClient animalesApi) => _animalesApi = animalesApi;

    [HttpGet("/Animales/{id:int:min(1)}")]
    public async Task<IActionResult> Detalle(int id, CancellationToken ct)
    {
        try
        {
            var detalle = await _animalesApi.ObtenerDetalleAsync(id, ct);
            if (detalle is null)
                return RedirectToAction("Index", "Home");

            return Inertia.Render("Animals/AnimalDetailPage", new
            {
                animal = detalle.Animal,
                vacunaciones = detalle.Vacunaciones,
                consultas = detalle.Consultas,
                apiError = (string?)null
            });
        }
        catch (Exception ex)
        {
            return Inertia.Render("Animals/AnimalDetailPage", new
            {
                animal = (object?)null,
                vacunaciones = Array.Empty<VacunacionDto>(),
                consultas = Array.Empty<ConsultaDto>(),
                apiError = ex.Message
            });
        }
    }
}
