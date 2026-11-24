using Abstracciones.TiposVacunas;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.TiposVacunas.TipoVacunaDtos;

namespace Flujo.TiposVacunas
{
    internal class TipoVacunaMapeador
    {

        public static TipoVacunaDto ADto(TipoVacuna a) => new(
        a.IdTipoVacuna,
        a.Nombre,
        a.Obligatoria,
        a.Activo);



    }
}
