using Abstracciones.Seguimientos;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Seguimientos.SeguimientoDtos;

namespace AdopcionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientosController : ControllerBase
    {
        private readonly ISeguimientoFlujo _flujo;
        public SeguimientosController(ISeguimientoFlujo flujo) => _flujo = flujo;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SeguimientoDto>>> Listar([FromQuery] SeguimientoFiltro filtro, CancellationToken ct)
        {
            var r = await _flujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = r.Total.ToString();
            Response.Headers["X-Page"] = r.Pagina.ToString();
            Response.Headers["X-Page-Size"] = r.Tamano.ToString();
            return Ok(r.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<SeguimientoDto>> Obtener(int id, CancellationToken ct) => Ok(await _flujo.ObtenerAsync(id, ct));

        [HttpPatch("{id:int:min(1)}/resultado")]
        public async Task<IActionResult> RegistrarResultado(int id, SeguimientoResultadoDTO dto, CancellationToken ct)
        {
            await _flujo.RegistrarResultadoAsync(id, dto, ct);
            return NoContent();
        }

        [HttpGet("alertas")]
        public async Task<ActionResult<IReadOnlyList<AlertaDto>>> Alertas(CancellationToken ct) => Ok(await _flujo.ListarAlertasAsync(ct));
    }
}
