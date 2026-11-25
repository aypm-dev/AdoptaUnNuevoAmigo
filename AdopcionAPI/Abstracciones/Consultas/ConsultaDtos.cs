using Abstracciones.Comunes;
using Abstracciones.Comunes.Validaciones;
using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Consultas
{
    public class ConsultaDtos
    {
        public class ConsultaCrearDTO
        {
            [Range(1, int.MaxValue, ErrorMessage = "La veterinaria/médico veterinario es obligatorio.")]
            public int VeterinariaId { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "El animal es obligatorio.")]
            public int AnimalId { get; set; }

            [Required(ErrorMessage = "La fecha es obligatoria.")]
            [FechaNoFutura(ErrorMessage = "La fecha de la consulta no puede estar en el futuro.")]
            public DateTime Fecha { get; set; }

            [Required(ErrorMessage = "El diagnóstico es obligatorio.")]
            [StringLength(1000, MinimumLength = 3, ErrorMessage = "El diagnóstico debe tener entre 3 y 1000 caracteres.")]
            public string Diagnostico { get; set; } = string.Empty;

            [Required(ErrorMessage = "El tratamiento es obligatorio.")]
            [StringLength(1000, MinimumLength = 3, ErrorMessage = "El tratamiento debe tener entre 3 y 1000 caracteres.")]
            public string Tratamiento { get; set; } = string.Empty;
        }

        // Veterinario y animal de un registro clínico no se reasignan; se corrige lo demás.
        public class ConsultaActualizarDTO
        {
            [Required(ErrorMessage = "La fecha es obligatoria.")]
            [FechaNoFutura(ErrorMessage = "La fecha de la consulta no puede estar en el futuro.")]
            public DateTime Fecha { get; set; }

            [Required(ErrorMessage = "El diagnóstico es obligatorio.")]
            [StringLength(1000, MinimumLength = 3, ErrorMessage = "El diagnóstico debe tener entre 3 y 1000 caracteres.")]
            public string Diagnostico { get; set; } = string.Empty;

            [Required(ErrorMessage = "El tratamiento es obligatorio.")]
            [StringLength(1000, MinimumLength = 3, ErrorMessage = "El tratamiento debe tener entre 3 y 1000 caracteres.")]
            public string Tratamiento { get; set; } = string.Empty;
        }

        public record ConsultaDto(int IdConsulta, int VeterinariaId, string? VeterinariaNombre,
            int AnimalId, string? AnimalNombre, DateTime Fecha, string Diagnostico, string Tratamiento);

        public class ConsultaFiltro : ParametrosPaginacion
        {
            public int? AnimalId { get; set; }
            public int? VeterinariaId { get; set; }
            public string Orden { get; set; } = "-fecha";
        }
    }
}
