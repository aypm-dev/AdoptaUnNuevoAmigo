using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abstracciones.VisitasDomiciliarias
{
    public class VisitaDtos
    {

        public class VisitaCrearDTO
        {
            [Range(1, int.MaxValue)] public int SolicitudId { get; set; }
            [Range(1, int.MaxValue)] public int VoluntarioId { get; set; }
            public DateTime Fecha { get; set; }   // UTC (ISO 8601 con "Z")
        }

        public class VisitaResultadoDTO
        {
            [EnumDataType(typeof(ResultadoVisita))] public ResultadoVisita Resultado { get; set; }
            [StringLength(1000)] public string? Observaciones { get; set; }
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
