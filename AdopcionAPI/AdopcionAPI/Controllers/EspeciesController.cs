using Abstracciones.Especies;
using Abstracciones.Modelos;
using DA.Contexto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdopcionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspeciesController : ControllerBase
    {
        private readonly IEspecieFlujo _especieFlujo;

        public EspeciesController(IEspecieFlujo especieFlujo) => _especieFlujo = especieFlujo;


        [HttpGet]
        [ProducesResponseType<IReadOnlyList<IEspecieDtos.EspecieDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<IEspecieDtos.EspecieDto>>> Listar([FromQuery] IEspecieDtos.EspecieFiltro filtro, CancellationToken ct)
        {
            var resultado = await _especieFlujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = resultado.Total.ToString();
            Response.Headers["X-Page"] = resultado.Pagina.ToString();
            Response.Headers["X-Page-Size"] = resultado.Tamano.ToString();
            return Ok(resultado.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType<IEspecieDtos.EspecieDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEspecieDtos.EspecieDto>> Obtener(int id, CancellationToken ct) =>
            Ok(await _especieFlujo.ObtenerAsync(id, ct));

        [HttpPost]
        [ProducesResponseType<IEspecieDtos.EspecieDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IEspecieDtos.EspecieDto>> Crear(IEspecieDtos.EspecieCrearDTO dto, CancellationToken ct)
        {
            var creado = await _especieFlujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Actualizar(int id, IEspecieDtos.EspecieActualizarDTO dto, CancellationToken ct)
        {
            await _especieFlujo.ActualizarAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            await _especieFlujo.EliminarAsync(id, ct);
            return NoContent();
        }

    }
}