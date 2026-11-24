using Abstracciones.Especies;
using Abstracciones.TiposViviendas;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.TiposViviendas.TipoViviendaDtos;

namespace Flujo.TiposViviendas
{
    public class TipoViviendaMapeador
    {

        public static TipoViviendaDto ADto(TipoVivienda t) =>
            new(t.IdTipoVivienda, t.Nombre, t.Descripcion, t.Activo, t.FechaRegistro);

    }
}
