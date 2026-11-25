using Abstracciones.VisitasDomiciliarias;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.VisitasDomiciliarias.VisitaDtos;

namespace AdopcionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitasController : ControllerBase
    {

        private readonly IVisitaFlujo _flujo;
        public VisitasController(IVisitaFlujo flujo) => _flujo = flujo;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VisitaDto>>> Listar([FromQuery] VisitaFiltro filtro, CancellationToken ct)
        {
            var r = await _flujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = r.Total.ToString();
            Response.Headers["X-Page"] = r.Pagina.ToString();
            Response.Headers["X-Page-Size"] = r.Tamano.ToString();
            return Ok(r.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<VisitaDto>> Obtener(int id, CancellationToken ct) => Ok(await _flujo.ObtenerAsync(id, ct));

        [HttpPost]
        public async Task<ActionResult<VisitaDto>> Crear(VisitaCrearDTO dto, CancellationToken ct)
        {
            var c = await _flujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = c.IdVisita }, c);
        }

        [HttpPatch("{id:int:min(1)}/resultado")]
        public async Task<IActionResult> RegistrarResultado(int id, VisitaResultadoDTO dto, CancellationToken ct)
        {
            await _flujo.RegistrarResultadoAsync(id, dto, ct);
            return NoContent();
        }

    }
}
