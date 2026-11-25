using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Vacunas.VacunacionDtos;

namespace Abstracciones.Vacunas
{
    public interface IVacunacionDA
    {
        Task<ResultadoPaginado<Vacunacion>> BuscarAsync(VacunacionFiltro f, CancellationToken ct);
        Task<Vacunacion?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<Vacunacion> AgregarAsync(Vacunacion v, CancellationToken ct);
        Task<bool> ActualizarAsync(Vacunacion datos, CancellationToken ct);
        Task<bool> TipoVacunaExisteAsync(int tipoVacunaId, CancellationToken ct);
        /// <summary>True si por cada vacuna obligatoria el animal tiene una aplicación vigente a la fecha dada.</summary>
        Task<bool> TieneVacunasObligatoriasAlDiaAsync(int animalId, DateTime fecha, CancellationToken ct);

    }
}
