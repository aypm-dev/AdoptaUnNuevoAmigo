using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abstracciones.Veterinarias
{
    public class VeterinariaDtos
    {

        public class VeterinariaCrearDTO
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
            public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
            public string Nombre { get; set; } = string.Empty;
            public string Diagnostico { get; set; } = string.Empty;
            public string Tratamiento  { get; set; } = string.Empty;
            public bool Activo { get; set; }

        }



        public class VeterinariaActualizarDTO : VeterinariaCrearDTO { }

        /// <summary>Salida hacia el cliente HTTP. Nunca se expone la entidad directamente.</summary>
        public record VeterinariaDto(
            int Id,
            string Nombre,
            DateTime FechaRegsitro,
            string Diagnostico,
            string Tratamiento,
            bool Activo);

        public class VeterinariaFiltro : ParametrosPaginacion
        {
            /// <summary>Texto buscado en nombre, correo o empresa.</summary>
            [StringLength(100)]
            public string? Q { get; set; }
        }

    }
}
