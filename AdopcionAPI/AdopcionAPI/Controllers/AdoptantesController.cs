using Abstracciones.Adoptantes;
using Abstracciones.Animales;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Adoptantes.AdoptanteDtos;
using static Abstracciones.Animales.AnimalDtos;

namespace AdopcionAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdoptantesController : ControllerBase
    {

        private readonly IAdoptanteFlujo _adoptanteFlujo;

        public AdoptantesController(IAdoptanteFlujo adoptanteFlujo) => _adoptanteFlujo = adoptanteFlujo;


        [HttpGet]
        [ProducesResponseType<IReadOnlyList<AdoptanteDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<AdoptanteDto>>> Listar([FromQuery] AdoptanteFiltro filtro, CancellationToken ct)
        {
            var resultado = await _adoptanteFlujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = resultado.Total.ToString();
            Response.Headers["X-Page"] = resultado.Pagina.ToString();
            Response.Headers["X-Page-Size"] = resultado.Tamano.ToString();
            return Ok(resultado.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType<AdoptanteDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdoptanteDto>> Obtener(int id, CancellationToken ct) =>
            Ok(await _adoptanteFlujo.ObtenerAsync(id, ct));

        [HttpPost]
        [ProducesResponseType<AdoptanteDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AdoptanteDto>> Crear(AdoptanteCrearDTO dto, CancellationToken ct)
        {
            var creado = await _adoptanteFlujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creado.IdAdoptante }, creado);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Actualizar(int id, AdoptanteActualizarDTO dto, CancellationToken ct)
        {
            await _adoptanteFlujo.ActualizarAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            await _adoptanteFlujo.EliminarAsync(id, ct);
            return NoContent();
        }


    }
}
