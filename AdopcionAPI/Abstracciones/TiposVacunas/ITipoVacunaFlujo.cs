using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Especies.IEspecieDtos;
using static Abstracciones.TiposVacunas.TipoVacunaDtos;

namespace Abstracciones.TiposVacunas
{
    public interface ITipoVacunaFlujo
    {

        Task<ResultadoPaginado<TipoVacunaDto>> ListarAsync(TipoVacunaFiltro filtro, CancellationToken ct);
        Task<TipoVacunaDto> ObtenerAsync(int id, CancellationToken ct);
        Task<TipoVacunaDto> CrearAsync(TipoVacunaCrearDTO dto, CancellationToken ct);
        Task ActualizarAsync(int id, TipoVacunaActualizarDTO dto, CancellationToken ct);
        Task EliminarAsync(int id, CancellationToken ct);

    }
}
