using System;

namespace Abstracciones.Excepciones;

public abstract class ExcepcionDeNegocio : Exception
{
    protected ExcepcionDeNegocio(string mensaje) : base(mensaje) { }
}

public class NoEncontradoException : ExcepcionDeNegocio
{
    public NoEncontradoException(string entidad, object id)
        : base($"{entidad} con identificador '{id}' no existe.") { }

    public NoEncontradoException(string mensaje)
        : base(mensaje) { }
}

/// <summary>Alias estándar para NoEncontradoException.</summary>
public class EntidadNoEncontradaException : NoEncontradoException
{
    public EntidadNoEncontradaException(string entidad, object id) : base(entidad, id) { }
    public EntidadNoEncontradaException(string mensaje) : base(mensaje) { }
}

/// <summary>La operación choca con el estado actual (duplicado, transición inválida, regla). HTTP 409.</summary>
public class ConflictoException : ExcepcionDeNegocio
{
    public ConflictoException(string mensaje) : base(mensaje) { }
}

/// <summary>Conflicto de concurrencia optimista o estado concurrente. HTTP 409.</summary>
public class ConflictoConcurrenciaException : ConflictoException
{
    public ConflictoConcurrenciaException(string mensaje) : base(mensaje) { }
}

/// <summary>El cuerpo es válido en forma, pero referencia algo inexistente o rompe una regla de negocio. HTTP 400/422.</summary>
public class ValidacionNegocioException : ExcepcionDeNegocio
{
    public ValidacionNegocioException(string mensaje) : base(mensaje) { }
}

/// <summary>Violación de regla de negocio general. HTTP 400.</summary>
public class ReglaDeNegocioException : ValidacionNegocioException
{
    public ReglaDeNegocioException(string mensaje) : base(mensaje) { }
}
