using Abstracciones.Comunes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Razas
{
    public class RazaDtos
    {
        public class RazaCrearDTO
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
            public string Nombre { get; set; } = string.Empty;

            [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
            public string Descripcion { get; set; } = string.Empty;

            [Range(1, int.MaxValue, ErrorMessage = "La especie es obligatoria.")]
            public int EspecieId { get; set; }
        }

        public class RazaActualizarDTO : RazaCrearDTO { }

        /// <summary>Salida hacia el cliente HTTP. Nunca se expone la entidad directamente.</summary>
        public record RazaDto(
            int IdRaza,
            string Nombre,
            string Descripcion,
            int EspecieId,
            string EspecieNombre,
            DateTime FechaRegistro);

        public class RazaFiltro : ParametrosPaginacion
        {
            /// <summary>Texto buscado en nombre o descripción.</summary>
            [StringLength(100)]
            public string? Q { get; set; }

            public int? EspecieId { get; set; }

            public string Orden { get; set; } = "-fecha";
        }
    }
}
