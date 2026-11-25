using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Abstracciones.Comunes.Validaciones;

/// <summary>
/// Valida que la identificación del adoptante o usuario cumpla con un formato válido
/// (entre 9 y 20 caracteres alfanuméricos, admitiendo guiones habituales en identificaciones).
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class IdentificacionValidaAttribute : ValidationAttribute
{
    public IdentificacionValidaAttribute()
        : base("La identificación debe tener un formato válido (entre 9 y 20 caracteres sin espacios).")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var str = value.ToString()?.Trim();
        if (string.IsNullOrEmpty(str))
            return ValidationResult.Success;

        if (!Regex.IsMatch(str, @"^[A-Za-z0-9\-]{9,20}$"))
        {
            return new ValidationResult(ErrorMessage ?? "La identificación debe contener entre 9 y 20 caracteres válidos.");
        }

        return ValidationResult.Success;
    }
}
