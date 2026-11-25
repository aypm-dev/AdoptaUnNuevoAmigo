using Abstracciones.Comunes;
using Abstracciones.Comunes.Validaciones;
using System;
using System.ComponentModel.DataAnnotations;
using static Abstracciones.Razas.RazaDtos;

namespace Abstracciones.Animales
{
    public class AnimalDtos
    {
        public class AnimalCrearDTO
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
            public string Nombre { get; set; } = string.Empty;

            [Required(ErrorMessage = "El sexo es obligatorio.")]
            [StringLength(20, ErrorMessage = "El sexo no puede exceder los 20 caracteres.")]
            public string Sexo { get; set; } = string.Empty;

            [Required(ErrorMessage = "El estado de esterilización es obligatorio.")]
            [StringLength(20, ErrorMessage = "El campo esterilizado no puede exceder los 20 caracteres.")]
            public string Esterilizado { get; set; } = string.Empty;

            [Required(ErrorMessage = "El tamaño es obligatorio.")]
            [StringLength(50, ErrorMessage = "El tamaño no puede exceder los 50 caracteres.")]
            public string Tamanio { get; set; } = string.Empty;

            [Range(1, int.MaxValue, ErrorMessage = "La raza es obligatoria.")]
            public int RazaId { get; set; }

            [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
            [FechaNoFutura(ErrorMessage = "La fecha de nacimiento no puede estar en el futuro.")]
            public DateTime FechaNacimiento { get; set; }

            public bool Activo { get; set; } = true;
        }

        public class AnimalActualizarDTO : AnimalCrearDTO { }

        public record AnimalDto(
            int IdAnimal,
            string Nombre,
            int RazaId,
            DateTime FechaNacimiento,
            string Sexo,
            string Tamanio,
            string Esterilizado,
            string? RazaNombre,
            bool Activo,
            EstadoAnimal Estado
        );

        public class AnimalFiltro : ParametrosPaginacion
        {
            /// <summary>Texto buscado en nombre o raza.</summary>
            [StringLength(100)]
            public string? Q { get; set; }

            public int? RazaId { get; set; }

            public string Orden { get; set; } = "-fecha";
        }
    }
}
