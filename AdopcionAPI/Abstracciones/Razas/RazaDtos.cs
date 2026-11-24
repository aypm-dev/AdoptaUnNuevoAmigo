using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Abstracciones.TiposViviendas.TipoViviendaDtos;

namespace Abstracciones.Razas
{
    public class RazaDtos
    {

        public class RazaCrearDTO
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
            public string Nombre { get; set; } = string.Empty;

            public string Descripcion { get; set; } = string.Empty;

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
            /// <summary>Texto buscado en nombre, correo o empresa.</summary>
            [StringLength(100)]
            public string? Q { get; set; }

            public int? EspecieId { get; set; }

            public string Orden { get; set; } = "-fecha";


        }


    }
}
