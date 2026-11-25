using Abstracciones.Solicitudes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Solicitudes.SolicitudAdopcionDtos;

namespace Flujo.Solicitudes
{
    public class SolicitudMapeador
    {

        public static SolicitudDto ADto(SolicitudAdopcion s) => new(
           s.IdSolicitud, s.AnimalId, s.Animal?.Nombre,
           s.AdoptanteId, s.Adoptante?.Nombre,
           s.FechaSolicitud, s.FechaResolucion, s.Estado);

    }
}
