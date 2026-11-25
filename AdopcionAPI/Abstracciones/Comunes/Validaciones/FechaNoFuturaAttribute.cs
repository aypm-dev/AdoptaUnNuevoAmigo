using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Comunes.Validaciones;

/// <summary>
/// Valida que una fecha no se encuentre en el futuro.
/// Aplica a fechas de nacimiento, fechas de vacunación y consultas clínicas históricas.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FechaNoFuturaAttribute : ValidationAttribute
{
    public FechaNoFuturaAttribute() 
        : base("La fecha no puede estar en el futuro.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        if (value is DateTime fecha)
        {
            // Permitir un margen de 1 minuto para diferencias de reloj
            if (fecha > DateTime.UtcNow.AddMinutes(1))
            {
                return new ValidationResult(ErrorMessage ?? "La fecha no puede estar en el futuro.");
            }
        }
        else if (value is DateOnly fechaSolo)
        {
            if (fechaSolo > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return new ValidationResult(ErrorMessage ?? "La fecha no puede estar en el futuro.");
            }
        }

        return ValidationResult.Success;
    }
}
