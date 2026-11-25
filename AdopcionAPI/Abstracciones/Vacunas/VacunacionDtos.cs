using Abstracciones.Comunes;
using Abstracciones.Comunes.Validaciones;
using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Vacunas
{
    public class VacunacionDtos
    {
        public class VacunacionCrearDTO
        {
            [Range(1, int.MaxValue, ErrorMessage = "El animal es obligatorio.")]
            public int AnimalId { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "El tipo de vacuna es obligatorio.")]
            public int TipoVacunaId { get; set; }

            [Required(ErrorMessage = "La fecha de vacunación es obligatoria.")]
            [FechaNoFutura(ErrorMessage = "La fecha de vacunación no puede estar en el futuro.")]
            public DateTime Fecha { get; set; }

            public DateTime ProximaDosis { get; set; }
        }

        // El animal NO se puede cambiar al editar un registro.
        public class VacunacionActualizarDTO
        {
            [Range(1, int.MaxValue, ErrorMessage = "El tipo de vacuna es obligatorio.")]
            public int TipoVacunaId { get; set; }

            [Required(ErrorMessage = "La fecha de vacunación es obligatoria.")]
            [FechaNoFutura(ErrorMessage = "La fecha de vacunación no puede estar en el futuro.")]
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
