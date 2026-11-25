using System.ComponentModel.DataAnnotations;

namespace AdopcionAPI.Errores;

/// <summary>
/// En Controllers, [ApiController] valida las anotaciones y responde 400 automáticamente.
/// En Minimal API eso no ocurre por defecto, por lo que este helper permite validar a mano
/// y devolver el diccionario de errores estándar con el mismo formato ProblemDetails.
/// </summary>
public static class ValidacionHelper
{
    public static bool EsValido(object modelo, out Dictionary<string, string[]> errores)
    {
        var resultados = new List<ValidationResult>();
        var contexto = new ValidationContext(modelo);
        Validator.TryValidateObject(modelo, contexto, resultados, validateAllProperties: true);

        errores = resultados
            .SelectMany(r => (r.MemberNames.Any() ? r.MemberNames : new[] { string.Empty })
                .Select(m => (Campo: m, Mensaje: r.ErrorMessage ?? "Valor inválido.")))
            .GroupBy(x => x.Campo)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Mensaje).ToArray());

        return errores.Count == 0;
    }
}
