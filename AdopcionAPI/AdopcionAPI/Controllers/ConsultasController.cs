using Abstracciones.Consultas;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Consultas.ConsultaDtos;

namespace AdopcionAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {

        private readonly IConsultaFlujo _flujo;
        public ConsultasController(IConsultaFlujo flujo) => _flujo = flujo;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ConsultaDto>>> Listar([FromQuery] ConsultaFiltro filtro, CancellationToken ct)
        {
            var r = await _flujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = r.Total.ToString();
            Response.Headers["X-Page"] = r.Pagina.ToString();
            Response.Headers["X-Page-Size"] = r.Tamano.ToString();
            return Ok(r.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ConsultaDto>> Obtener(int id, CancellationToken ct) => Ok(await _flujo.ObtenerAsync(id, ct));

        [HttpPost]
        public async Task<ActionResult<ConsultaDto>> Crear(ConsultaCrearDTO dto, CancellationToken ct)
        {
            var c = await _flujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = c.IdConsulta }, c);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> Actualizar(int id, ConsultaActualizarDTO dto, CancellationToken ct)
        {
            await _flujo.ActualizarAsync(id, dto, ct);
            return NoContent();
        }
    }

}

