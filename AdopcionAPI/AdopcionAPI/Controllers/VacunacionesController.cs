using Abstracciones.Vacunas;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Vacunas.VacunacionDtos;

namespace AdopcionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacunacionesController : ControllerBase
    {

        private readonly IVacunacionFlujo _flujo;
        public VacunacionesController(IVacunacionFlujo flujo) => _flujo = flujo;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VacunacionDto>>> Listar([FromQuery] VacunacionFiltro filtro, CancellationToken ct)
        {
            var r = await _flujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = r.Total.ToString();
            Response.Headers["X-Page"] = r.Pagina.ToString();
            Response.Headers["X-Page-Size"] = r.Tamano.ToString();
            return Ok(r.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<VacunacionDto>> Obtener(int id, CancellationToken ct) => Ok(await _flujo.ObtenerAsync(id, ct));

        [HttpPost]
        public async Task<ActionResult<VacunacionDto>> Crear(VacunacionCrearDTO dto, CancellationToken ct)
        {
            var c = await _flujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = c.IdVacunacion }, c);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> Actualizar(int id, VacunacionActualizarDTO dto, CancellationToken ct)
        {
            await _flujo.ActualizarAsync(id, dto, ct);
            return NoContent();
        }
    }

}