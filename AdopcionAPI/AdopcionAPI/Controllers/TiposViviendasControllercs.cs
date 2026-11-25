using Abstracciones.Especies;
using Abstracciones.TiposViviendas;
using Azure;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.TiposViviendas.TipoViviendaDtos;

namespace AdopcionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposViviendasControllercs: ControllerBase
    {

        private readonly ITipoViviendaFlujo _tipoViviendaFlujo;

        public TiposViviendasControllercs(ITipoViviendaFlujo tipoViviendaFlujo) => _tipoViviendaFlujo = tipoViviendaFlujo;


        [HttpGet]
        [ProducesResponseType<IReadOnlyList<TipoViviendaDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<TipoViviendaDto>>> Listar([FromQuery] TipoViviendaFiltro filtro, CancellationToken ct)
        {
            var resultado = await _tipoViviendaFlujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = resultado.Total.ToString();
            Response.Headers["X-Page"] = resultado.Pagina.ToString();
            Response.Headers["X-Page-Size"] = resultado.Tamano.ToString();
            return Ok(resultado.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType<TipoViviendaDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipoViviendaDto>> Obtener(int id, CancellationToken ct) =>
            Ok(await _tipoViviendaFlujo.ObtenerAsync(id, ct));

        [HttpPost]
        [ProducesResponseType<TipoViviendaDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<TipoViviendaDto>> Crear(TipoViviendaCrearDTO dto, CancellationToken ct)
        {
            var creado = await _tipoViviendaFlujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Actualizar(int id, TipoViviendaActualizarDTO   dto, CancellationToken ct)
        {
            await _tipoViviendaFlujo.ActualizarAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            await _tipoViviendaFlujo.EliminarAsync(id, ct);
            return NoContent();
        }

    }
}
