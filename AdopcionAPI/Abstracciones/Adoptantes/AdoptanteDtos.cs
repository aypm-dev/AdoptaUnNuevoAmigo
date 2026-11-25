using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abstracciones.Adoptantes
{
    public class AdoptanteDtos
    {

        public class AdoptanteCrearDTO
        {

            public string Identificacion { get; set; }

            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
            public string Nombre { get; set; } = string.Empty;

            public string Contacto { get; set; }
            public string OtrasMascotas { get; set; }
            public int TipoViviendaId { get; set; }

            public bool Activo { get; set; }

            public DateTime FechaRegistro { get; set; }
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
            /// <summary>Texto buscado en nombre, correo o empresa.</summary>
            [StringLength(100)]
            public string? Q { get; set; }

            public int? TipoViviendaId { get; set; }

            public string Orden { get; set; } = "-fecha";


        }


    }
}
