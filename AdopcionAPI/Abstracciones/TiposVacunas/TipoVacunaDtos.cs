using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abstracciones.TiposVacunas
{
    public class TipoVacunaDtos
    {

       
        public class TipoVacunaCrearDTO
        {
            public string Nombre { get; set; } = null!;
            public bool Obligatoria { get; set; }
            public bool Activo { get; set; }
        }
        public class TipoVacunaActualizarDTO : TipoVacunaCrearDTO { }

        public record TipoVacunaDto(
           int IdTipoVacuna,
           string Nombre,
           bool Obligatoria,
           bool Activo);

        public class TipoVacunaFiltro : ParametrosPaginacion
        {
            /// <summary>Texto buscado en nombre, correo o empresa.</summary>
            [StringLength(100)]
            public string? Q { get; set; }
        }


    }
}
