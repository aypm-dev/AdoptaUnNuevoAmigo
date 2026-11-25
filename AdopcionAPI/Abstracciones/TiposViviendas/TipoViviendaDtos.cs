using Abstracciones.Comunes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.TiposViviendas
{
    public class TipoViviendaDtos
    {
        public class TipoViviendaCrearDTO
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
            public string Nombre { get; set; } = string.Empty;

            [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
            public string Descripcion { get; set; } = string.Empty;

            public bool Activo { get; set; } = true;
        }

        public class TipoViviendaActualizarDTO : TipoViviendaCrearDTO { }

        /// <summary>Salida hacia el cliente HTTP. Nunca se expone la entidad directamente.</summary>
        public record TipoViviendaDto(
            int Id,
            string Nombre,
            string Descripcion,
            bool Activo,
            DateTime FechaRegistro);

        public class TipoViviendaFiltro : ParametrosPaginacion
        {
            /// <summary>Texto buscado en nombre o descripción.</summary>
            [StringLength(100)]
            public string? Q { get; set; }
        }
    }
}
