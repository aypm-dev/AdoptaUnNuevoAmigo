using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abstracciones.Consultas
{
    public class ConsultaDtos
    {

        public class ConsultaCrearDTO
        {
            [Range(1, int.MaxValue)] public int VeterinariaId { get; set; }
            [Range(1, int.MaxValue)] public int AnimalId { get; set; }
            public DateTime Fecha { get; set; }
            [Required, StringLength(1000, MinimumLength = 3)] public string Diagnostico { get; set; } = string.Empty;
            [Required, StringLength(1000, MinimumLength = 3)] public string Tratamiento { get; set; } = string.Empty;
        }

        // Veterinario y animal de un registro clínico no se reasignan; se corrige lo demás.
        public class ConsultaActualizarDTO
        {
            public DateTime Fecha { get; set; }
            [Required, StringLength(1000, MinimumLength = 3)] public string Diagnostico { get; set; } = string.Empty;
            [Required, StringLength(1000, MinimumLength = 3)] public string Tratamiento { get; set; } = string.Empty;
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
