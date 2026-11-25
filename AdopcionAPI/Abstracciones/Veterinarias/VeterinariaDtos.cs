using Abstracciones.Comunes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Veterinarias
{
    public class VeterinariaDtos
    {
        public class VeterinariaCrearDTO
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
            public string Nombre { get; set; } = string.Empty;

            public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

            [StringLength(500, ErrorMessage = "El diagnóstico no puede superar los 500 caracteres.")]
            public string Diagnostico { get; set; } = string.Empty;

            [StringLength(500, ErrorMessage = "El tratamiento no puede superar los 500 caracteres.")]
            public string Tratamiento { get; set; } = string.Empty;

            public bool Activo { get; set; } = true;
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
            /// <summary>Texto buscado en nombre o diagnóstico.</summary>
            [StringLength(100)]
            public string? Q { get; set; }
        }
    }
}
