using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abstracciones.Vacunas
{
    public class VacunacionDtos
    {

        public class VacunacionCrearDTO
        {
            [Range(1, int.MaxValue)] public int AnimalId { get; set; }
            [Range(1, int.MaxValue)] public int TipoVacunaId { get; set; }
            public DateTime Fecha { get; set; }
            public DateTime ProximaDosis { get; set; }
        }

        // El animal NO se puede cambiar al editar un registro.
        public class VacunacionActualizarDTO
        {
            [Range(1, int.MaxValue)] public int TipoVacunaId { get; set; }
            public DateTime Fecha { get; set; }
            public DateTime ProximaDosis { get; set; }
        }

        public record VacunacionDto(int IdVacunacion, int AnimalId, string? AnimalNombre,
            int TipoVacunaId, string? TipoVacunaNombre, DateTime Fecha, DateTime? ProximaDosis);

        public class VacunacionFiltro : ParametrosPaginacion
        {
            public int? AnimalId { get; set; }
            public int? TipoVacunaId { get; set; }
            public string Orden { get; set; } = "-fecha";
        }

    }
}
