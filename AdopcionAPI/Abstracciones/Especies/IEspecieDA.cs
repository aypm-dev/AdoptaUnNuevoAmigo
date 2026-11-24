using Abstracciones.Comunes;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Especies.IEspecieDtos;

namespace Abstracciones.Especies
{
    public interface IEspecieDA
    {

        Task<ResultadoPaginado<Especie>> BuscarAsync(EspecieFiltro filtro, CancellationToken ct);
        Task<Especie?> ObtenerPorIdAsync(int id, CancellationToken ct);
        Task<Especie> AgregarAsync(Especie especie, CancellationToken ct);

        /// <summary>Actualiza Nombre, Correo, Telefono y Empresa. Devuelve false si el cliente no existe.</summary>
        Task<bool> ActualizarAsync(Especie datos, CancellationToken ct);

        Task<bool> EliminarAsync(int id, CancellationToken ct);

        /// <summary>Ejemplo didáctico de procedimiento almacenado con parámetros (sin concatenar SQL).</summary>

    }
}
