using Abstracciones.Animales;
using AdopcionWeb.Clients;
using AdopcionWeb.Models;
using InertiaCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Abstracciones.Animales.AnimalDtos;
using static Abstracciones.Especies.IEspecieDtos;
using static Abstracciones.Razas.RazaDtos;
using static Abstracciones.Vacunas.VacunacionDtos;

namespace AdopcionWeb.Controllers;

public class HomeController : Controller
{
    private readonly CatalogoApiClient _catalogoApi;

    public HomeController(CatalogoApiClient catalogoApi) => _catalogoApi = catalogoApi;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        IReadOnlyList<AnimalDto> animales = [];
        IReadOnlyList<EspecieDto> especies = [];
        IReadOnlyList<RazaDto> razas = [];
        IReadOnlyList<VacunacionDto> vacunaciones = [];
        string? apiError = null;

        try
        {
            var datos = await _catalogoApi.ObtenerCatalogoAsync(ct);

            // HU-03: Solo los animales activos con estado Disponible aparecen en el catálogo público.
            animales = datos.Animales
                .Where(a => a.Activo && a.Estado == EstadoAnimal.Disponible)
                .ToList();
            especies = datos.Especies.Where(e => e.Activo).ToList();
            razas = datos.Razas;
            vacunaciones = datos.Vacunaciones;
        }
        catch (Exception ex)
        {
            apiError = ex.Message;
        }

        return Inertia.Render("Catalog/CatalogPage", new
        {
            animales,
            especies,
            razas,
            vacunaciones,
            apiError
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
