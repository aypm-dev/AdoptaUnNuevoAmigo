using Abstracciones.Seguimientos;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Seguimientos.SeguimientoDtos;

namespace Flujo.Seguimientos
{
    internal class SeguimientoMapeador
    {

        public static SeguimientoDto ADto(SeguimientoPostAdopcion s) => new(
           s.IdSeguimiento, s.SolicitudId, s.Solicitud?.AnimalId ?? 0, s.Solicitud?.Animal?.Nombre,
           s.Solicitud?.Adoptante?.Nombre, s.Hito, s.FechaProgramada, s.FechaRealizada, s.Estado, s.Observaciones);

        public static AlertaDto ADto(AlertaSeguimiento a) => new(
            a.IdAlerta, a.SeguimientoId, a.FechaCreacion, a.Mensaje, a.EscaladaAlCoordinador, a.FechaEscalada);

    }
}
