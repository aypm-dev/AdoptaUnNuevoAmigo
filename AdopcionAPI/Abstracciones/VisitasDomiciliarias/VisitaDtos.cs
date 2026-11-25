using Abstracciones.Comunes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.VisitasDomiciliarias
{
    public class VisitaDtos
    {
        public class VisitaCrearDTO
        {
            [Range(1, int.MaxValue, ErrorMessage = "La solicitud es obligatoria.")]
            public int SolicitudId { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "El voluntario es obligatorio.")]
            public int VoluntarioId { get; set; }

            [Required(ErrorMessage = "La fecha de la visita es obligatoria.")]
            public DateTime Fecha { get; set; }   // UTC (ISO 8601 con "Z")
        }

        public class VisitaResultadoDTO
        {
            [Required(ErrorMessage = "El resultado de la visita es obligatorio.")]
            [EnumDataType(typeof(ResultadoVisita), ErrorMessage = "El resultado no es válido.")]
            public ResultadoVisita Resultado { get; set; }

            [StringLength(1000, ErrorMessage = "Las observaciones no pueden superar los 1000 caracteres.")]
            public string? Observaciones { get; set; }
        }

        public record VisitaDto(int IdVisita, int SolicitudId, int VoluntarioId, string? VoluntarioNombre,
            DateTime Fecha, ResultadoVisita Resultado, string? Observaciones);

        public class VisitaFiltro : ParametrosPaginacion
        {
            public int? SolicitudId { get; set; }
            public int? VoluntarioId { get; set; }
            public ResultadoVisita? Resultado { get; set; }
            public string Orden { get; set; } = "-fecha";
        }
    }
}
