using Abstracciones.Comunes;
using Abstracciones.Especies;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Especies.IEspecieDtos;
using static Abstracciones.TiposVacunas.TipoVacunaDtos;

namespace Abstracciones.TiposVacunas
{
    public interface ITipoVacunaDA
    {

        Task<ResultadoPaginado<TipoVacuna>> BuscarAsync(TipoVacunaFiltro filtro, CancellationToken ct);
        Task<TipoVacuna?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<TipoVacuna> AgregarAsync(TipoVacuna tipoVacuna, CancellationToken ct);
        Task<bool> ActualizarAsync(TipoVacuna datos, CancellationToken ct);
        Task<bool> EliminarAsync(int id, CancellationToken ct);


    }
}
