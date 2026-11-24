using Abstracciones.Especies;
using Abstracciones.TiposVacunas;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.TiposVacunas.TipoVacunaDtos;

namespace AdopcionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposVacunasController : ControllerBase
    {
        
        private readonly ITipoVacunaFlujo _tipoVacunaFlujo;

        public TiposVacunasController(ITipoVacunaFlujo tipoVacunaFlujo) => _tipoVacunaFlujo = tipoVacunaFlujo;


        [HttpGet]
        [ProducesResponseType<IReadOnlyList<TipoVacunaDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<TipoVacunaDto>>> Listar([FromQuery] TipoVacunaFiltro filtro, CancellationToken ct)
        {
            var resultado = await _tipoVacunaFlujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = resultado.Total.ToString();
            Response.Headers["X-Page"] = resultado.Pagina.ToString();
            Response.Headers["X-Page-Size"] = resultado.Tamano.ToString();
            return Ok(resultado.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType<TipoVacunaDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipoVacunaDto>> Obtener(int id, CancellationToken ct) =>
            Ok(await _tipoVacunaFlujo.ObtenerAsync(id, ct));

        [HttpPost]
        [ProducesResponseType<TipoVacunaDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IEspecieDtos.EspecieDto>> Crear(TipoVacunaCrearDTO dto, CancellationToken ct)
        {
            var creado = await _tipoVacunaFlujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creado.IdTipoVacuna }, creado);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Actualizar(int id, TipoVacunaActualizarDTO dto, CancellationToken ct)
        {
            await _tipoVacunaFlujo.ActualizarAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            await _tipoVacunaFlujo.EliminarAsync(id, ct);
            return NoContent();
        }

    }
}
