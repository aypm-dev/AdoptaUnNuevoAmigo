using Abstracciones.Especies;
using Abstracciones.Veterinarias;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Veterinarias.VeterinariaDtos;

namespace AdopcionAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VeterinariasController : ControllerBase
    {

        private readonly IVeterinariaFlujo _veterinariaFlujo;
        public VeterinariasController(IVeterinariaFlujo veterinariaFlujo) => _veterinariaFlujo = veterinariaFlujo;
        [HttpGet]
        [ProducesResponseType<IReadOnlyList<VeterinariaDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<VeterinariaDto>>> Listar([FromQuery] VeterinariaFiltro filtro, CancellationToken ct)
        {
            var resultado = await _veterinariaFlujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = resultado.Total.ToString();
            Response.Headers["X-Page"] = resultado.Pagina.ToString();
            Response.Headers["X-Page-Size"] = resultado.Tamano.ToString();
            return Ok(resultado.Items);
        }


        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType<VeterinariaDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VeterinariaDto>> Obtener(int id, CancellationToken ct) =>
         Ok(await _veterinariaFlujo.ObtenerAsync(id, ct));

        [HttpPost]
        [ProducesResponseType<VeterinariaDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<VeterinariaDto>> Crear(VeterinariaCrearDTO dto, CancellationToken ct)
        {
            var creado = await _veterinariaFlujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Actualizar(int id, VeterinariaActualizarDTO dto, CancellationToken ct)
        {
            await _veterinariaFlujo.ActualizarAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            await _veterinariaFlujo.EliminarAsync(id, ct);
            return NoContent();
        }


    }
}
