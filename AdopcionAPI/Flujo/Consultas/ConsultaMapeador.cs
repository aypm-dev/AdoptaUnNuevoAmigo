using Abstracciones.Consultas;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Consultas.ConsultaDtos;

namespace Flujo.Consultas
{
    internal class ConsultaMapeador
    {
        public static ConsultaDto ADto(ConsultaVeterinaria c) => new(
           c.IdConsulta, c.VeterinariaId, c.Veterinaria?.Nombre, c.AnimalId, c.Animal?.Nombre,
           c.Fecha, c.Diagnostico, c.Tratamiento);

    }
}
