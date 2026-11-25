using Abstracciones.Comunes;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;

namespace DA.Comunes;

/// <summary>
/// Implementación EF Core del chequeo de salud. Solo DA conoce el DbContext.
/// </summary>
public class SaludBaseDatos : ISaludBaseDatos
{
    private readonly AdopcionContexto _contexto;

    public SaludBaseDatos(AdopcionContexto contexto) => _contexto = contexto;

    public Task<bool> EstaDisponibleAsync(CancellationToken ct = default) =>
        _contexto.Database.CanConnectAsync(ct);
}
