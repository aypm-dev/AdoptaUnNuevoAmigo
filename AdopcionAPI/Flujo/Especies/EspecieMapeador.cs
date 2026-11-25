using Abstracciones.Especies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flujo.Especies
{
    internal class EspecieMapeador
    {

        public static IEspecieDtos.EspecieDto ADto(Especie e) =>
            new(e.IdEspecie, e.Nombre, e.Descripcion, e.Activo, e.FechaRegistro);

    }
}
