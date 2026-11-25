using AdopcionWeb.Clients;
using AdopcionWeb.Models;
using InertiaCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Abstracciones.Especies.IEspecieDtos;

namespace AdopcionWeb.Controllers;

public class HomeController : Controller
{
    private readonly EspeciesApiClient _especiesApi;

    public HomeController(EspeciesApiClient especiesApi) => _especiesApi = especiesApi;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        IReadOnlyList<EspecieDto> especies = [];
        string? apiError = null;

        try
        {
            especies = await _especiesApi.ListarAsync(ct);
        }
        catch (Exception ex)
        {
            apiError = ex.Message;
        }

        return Inertia.Render("Home/Index", new
        {
            especies,
            apiError
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
