using System;
using System.Collections.Generic;
using System.Text;

namespace Abstracciones.TiposVacunas
{
    public class TipoVacuna
    {

        public int IdTipoVacuna { get; set; }
        public string Nombre { get; set; }
        public bool Obligatoria { get; set; }
        public bool Activo { get; set; } = true;

    }
}
