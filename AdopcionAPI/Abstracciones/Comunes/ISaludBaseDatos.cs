namespace Abstracciones.Comunes;

/// <summary>
/// Contrato para verificar la disponibilidad de la base de datos.
/// Vive en Abstracciones para que el API pueda exponer /salud sin conocer Entity Framework (respetando la regla de capas).
/// </summary>
public interface ISaludBaseDatos
{
    Task<bool> EstaDisponibleAsync(CancellationToken ct = default);
}
