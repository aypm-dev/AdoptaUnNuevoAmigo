using System;
using System.Collections.Generic;
using System.Text;

namespace Abstracciones.Excepciones
{
    public abstract class ExcepcionDeNegocio: Exception
    {
        protected ExcepcionDeNegocio(string mensaje) : base(mensaje) { }

    }

    public class NoEncontradoException : ExcepcionDeNegocio
    {
        public NoEncontradoException(string entidad, object id)
            : base($"{entidad} con identificador '{id}' no existe.") { }
    }

    /// <summary>La operación choca con el estado actual (duplicado, transición inválida, regla). HTTP 409.</summary>
    public class ConflictoException : ExcepcionDeNegocio
    {
        public ConflictoException(string mensaje) : base(mensaje) { }
    }

    /// <summary>El cuerpo es válido en forma, pero referencia algo inexistente o rompe una regla. HTTP 422.</summary>
    public class ValidacionNegocioException : ExcepcionDeNegocio
    {
        public ValidacionNegocioException(string mensaje) : base(mensaje) { }
    }

}
