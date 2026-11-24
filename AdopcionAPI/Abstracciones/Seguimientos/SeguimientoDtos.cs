using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abstracciones.Seguimientos
{
    public class SeguimientoDtos
    {

        public class SeguimientoResultadoDTO
        {
            public bool Favorable { get; set; }
            [StringLength(1000)] public string? Observaciones { get; set; }
        }

        public record SeguimientoDto(int IdSeguimiento, int SolicitudId, int AnimalId, string? AnimalNombre,
            string? AdoptanteNombre, HitoSeguimiento Hito, DateTime FechaProgramada,
            DateTime? FechaRealizada, EstadoSeguimiento Estado, string? Observaciones);

        public record AlertaDto(int IdAlerta, int SeguimientoId, DateTime FechaCreacion,
            string Mensaje, bool EscaladaAlCoordinador, DateTime? FechaEscalada);

        public class SeguimientoFiltro : ParametrosPaginacion
        {
            public int? SolicitudId { get; set; }
            public EstadoSeguimiento? Estado { get; set; }
            public HitoSeguimiento? Hito { get; set; }
            public string Orden { get; set; } = "fecha";   // por FechaProgramada
        }

    }
}
