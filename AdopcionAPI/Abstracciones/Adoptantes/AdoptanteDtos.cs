using Abstracciones.Comunes;
using Abstracciones.Comunes.Validaciones;
using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Adoptantes
{
    public class AdoptanteDtos
    {
        public class AdoptanteCrearDTO
        {
            [Required(ErrorMessage = "La identificación es obligatoria.")]
            [StringLength(20, MinimumLength = 9, ErrorMessage = "La identificación debe tener entre 9 y 20 caracteres.")]
            [IdentificacionValida(ErrorMessage = "La identificación debe contener entre 9 y 20 caracteres válidos sin espacios.")]
            public string Identificacion { get; set; } = string.Empty;

            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
            public string Nombre { get; set; } = string.Empty;

            [Required(ErrorMessage = "El contacto es obligatorio.")]
            [StringLength(150, MinimumLength = 5, ErrorMessage = "El contacto debe tener entre 5 y 150 caracteres.")]
            public string Contacto { get; set; } = string.Empty;

            [StringLength(500, ErrorMessage = "La descripción de otras mascotas no puede exceder los 500 caracteres.")]
            public string OtrasMascotas { get; set; } = string.Empty;

            [Range(1, int.MaxValue, ErrorMessage = "El tipo de vivienda es obligatorio.")]
            public int TipoViviendaId { get; set; }

            public bool Activo { get; set; } = true;

            public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        }

        public class AdoptanteActualizarDTO : AdoptanteCrearDTO { }

        public record AdoptanteDto(
            int IdAdoptante,
            string Identificacion,
            string Nombre,
            string Contacto,
            string OtrasMascotas,
            int TipoViviendaId,
            string? TipoViviendaNombre,
            DateTime FechaRegistro,
            bool Activo
        );

        public class AdoptanteFiltro : ParametrosPaginacion
        {
            /// <summary>Texto buscado en nombre, identificación o contacto.</summary>
            [StringLength(100)]
            public string? Q { get; set; }

            public int? TipoViviendaId { get; set; }

            public string Orden { get; set; } = "-fecha";
        }
    }
}
