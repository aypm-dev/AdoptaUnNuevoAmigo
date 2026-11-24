using Abstracciones.Animales;
using Abstracciones.Especies;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Animales.AnimalDtos;

namespace AdopcionAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AnimalesController  : ControllerBase
    {

        private readonly IAnimalFlujo _animalFlujo;

        public AnimalesController(IAnimalFlujo animalFlujo) => _animalFlujo = animalFlujo;


        [HttpGet]
        [ProducesResponseType<IReadOnlyList<AnimalDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<AnimalDto>>> Listar([FromQuery] AnimalFiltro filtro, CancellationToken ct)
        {
            var resultado = await _animalFlujo.ListarAsync(filtro, ct);
            Response.Headers["X-Total-Count"] = resultado.Total.ToString();
            Response.Headers["X-Page"] = resultado.Pagina.ToString();
            Response.Headers["X-Page-Size"] = resultado.Tamano.ToString();
            return Ok(resultado.Items);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType<AnimalDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AnimalDto>> Obtener(int id, CancellationToken ct) =>
            Ok(await _animalFlujo.ObtenerAsync(id, ct));

        [HttpPost]
        [ProducesResponseType<AnimalDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AnimalDto>> Crear(AnimalCrearDTO dto, CancellationToken ct)
        {
            var creado = await _animalFlujo.CrearAsync(dto, ct);
            return CreatedAtAction(nameof(Obtener), new { id = creado.IdAnimal }, creado);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Actualizar(int id, AnimalActualizarDTO dto, CancellationToken ct)
        {
            await _animalFlujo.ActualizarAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            await _animalFlujo.EliminarAsync(id, ct);
            return NoContent();
        }


    }

}
