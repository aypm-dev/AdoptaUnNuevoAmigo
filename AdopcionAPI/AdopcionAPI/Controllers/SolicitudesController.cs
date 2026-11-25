using Abstracciones.Solicitudes;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Solicitudes.SolicitudAdopcionDtos;

namespace AdopcionAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesController : ControllerBase
    {

        private readonly ISolicitudFlujo _flujo;
        public SolicitudesController(ISolicitudFlujo flujo) => _flujo = flujo;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SolicitudDto>>> Listar([FromQuery] SolicitudFiltro filtro, CancellationToken ct)
        {
            var r = await _flujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = r.Total.ToString();
            Response.Headers["X-Page"] = r.Pagina.ToString();
            Response.Headers["X-Page-Size"] = r.Tamano.ToString();
            return Ok(r.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<SolicitudDto>> Obtener(int id, CancellationToken ct) =>
            Ok(await _flujo.ObtenerAsync(id, ct));

        [HttpPost]
        [ProducesResponseType<SolicitudDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SolicitudDto>> Crear(SolicitudCrearDTO dto, CancellationToken ct)
        {
            var creada = await _flujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creada.IdSolicitud }, creada);
        }

        [HttpPatch("{id:int:min(1)}/estado")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CambiarEstado(int id, SolicitudCambiarEstadoDTO dto, CancellationToken ct)
        {
            await _flujo.CambiarEstadoAsync(id, dto.Estado, ct);
            return NoContent();
        }


    }
}
