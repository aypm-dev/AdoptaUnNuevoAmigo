using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abstracciones.Solicitudes
{
    public class SolicitudAdopcionDtos
    {

        public class SolicitudCrearDTO
        {
            [Range(1, int.MaxValue, ErrorMessage = "El animal es obligatorio.")]
            public int AnimalId { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "El adoptante es obligatorio.")]
            public int AdoptanteId { get; set; }
        }

        public class SolicitudCambiarEstadoDTO
        {
            [EnumDataType(typeof(EstadoSolicitud))]
            public EstadoSolicitud Estado { get; set; }
        }

        public record SolicitudDto(
            int IdSolicitud,
            int AnimalId,
            string? AnimalNombre,
            int AdoptanteId,
            string? AdoptanteNombre,
            DateTime FechaSolicitud,
            DateTime? FechaResolucion,
            EstadoSolicitud Estado);

        public class SolicitudFiltro : ParametrosPaginacion
        {
            public int? AnimalId { get; set; }
            public int? AdoptanteId { get; set; }
            public EstadoSolicitud? Estado { get; set; }
            public string Orden { get; set; } = "-fecha";
        }

    }
}
