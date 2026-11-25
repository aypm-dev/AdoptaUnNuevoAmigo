using Abstracciones.Comunes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.TiposVacunas
{
    public class TipoVacunaDtos
    {
        public class TipoVacunaCrearDTO
        {
            [Required(ErrorMessage = "El nombre de la vacuna es obligatorio.")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
            public string Nombre { get; set; } = string.Empty;

            public bool Obligatoria { get; set; }
            public bool Activo { get; set; } = true;
        }

        public class TipoVacunaActualizarDTO : TipoVacunaCrearDTO { }

        public record TipoVacunaDto(
           int IdTipoVacuna,
           string Nombre,
           bool Obligatoria,
           bool Activo);

        public class TipoVacunaFiltro : ParametrosPaginacion
        {
            /// <summary>Texto buscado en nombre.</summary>
            [StringLength(100)]
            public string? Q { get; set; }
        }
    }
}
