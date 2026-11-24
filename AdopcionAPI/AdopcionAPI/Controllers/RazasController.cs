using Abstracciones.Razas;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Razas.RazaDtos;

namespace AdopcionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RazasController : ControllerBase
    {

        private readonly IRazaFlujo _razaFlujo;

        public RazasController(IRazaFlujo razaFlujo) => _razaFlujo = razaFlujo;

        [HttpGet]
        [ProducesResponseType<IReadOnlyList<RazaDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<RazaDto>>> Listar([FromQuery] RazaFiltro filtro, CancellationToken ct)
        {
            var resultado = await _razaFlujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = resultado.Total.ToString();
            Response.Headers["X-Page"] = resultado.Pagina.ToString();
            Response.Headers["X-Page-Size"] = resultado.Tamano.ToString();
            return Ok(resultado.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType<RazaDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RazaDto>> Obtener(int id, CancellationToken ct)
        {
            var raza = await _razaFlujo.ObtenerAsync(id, ct);
            return Ok(raza);
        }

        [HttpPost]
        [ProducesResponseType<RazaDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<RazaDto>> Crear(RazaCrearDTO dto, CancellationToken ct)
        {
            var creado = await _razaFlujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creado.IdRaza }, creado);
        }


    }
}
