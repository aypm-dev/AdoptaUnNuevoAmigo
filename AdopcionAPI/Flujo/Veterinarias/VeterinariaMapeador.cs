using Abstracciones.Especies;
using Abstracciones.Veterinarias;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Veterinarias.VeterinariaDtos;

namespace Flujo.Veterinarias
{
    public class VeterinariaMapeador
    {

        public static VeterinariaDto ADto(Veterinaria v) =>
          new(v.IdVeterinaria, v.Nombre, v.FechaRegistro, v.Diagnostico, v.Tratamiento, v.Activo);

    }
}
