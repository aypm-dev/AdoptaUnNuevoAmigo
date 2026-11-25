using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
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

            public string Sexo { get; set; } = string.Empty;
            public string Esterilizado { get; set; } = string.Empty;
            public string Tamanio { get; set; } = string.Empty;
            public int RazaId { get; set; }

            public DateTime FechaNacimiento { get; set; }

            public bool Activo { get; set; }

            [StringLength(500)]
            public string? FotoUrl { get; set; }

            [StringLength(1000)]
            public string? Descripcion { get; set; }
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
            EstadoAnimal Estado,
            string? FotoUrl = null,
            string? Descripcion = null
        );

        public class AnimalFiltro : ParametrosPaginacion
        {
            /// <summary>Texto buscado en nombre, correo o empresa.</summary>
            [StringLength(100)]
            public string? Q { get; set; }

            public int? RazaId { get; set; }

            public string Orden { get; set; } = "-fecha";


        }

    }
}
