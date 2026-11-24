using Abstracciones.VisitasDomiciliarias;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.VisitasDomiciliarias.VisitaDtos;

namespace Flujo.VisitasDomiciliarias
{
    internal class VisitaMapeador
    {

        public static VisitaDto ADto(VisitaDomiciliaria v) => new(
            v.IdVisita, v.SolicitudId, v.VoluntarioId, v.Voluntario?.Nombre, v.Fecha, v.Resultado, v.Observaciones);

    }
}
