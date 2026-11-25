using Abstracciones.Vacunas;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Vacunas.VacunacionDtos;

namespace Flujo.VacunacionFlujo
{
    public class VacunacionMapeador
    {

        public static VacunacionDto ADto(Vacunacion v) => new(
           v.IdVacunacion, v.AnimalId, v.Animal?.Nombre,
           v.TipoVacunaId, v.TipoVacuna?.Nombre, v.Fecha, v.ProximaDosis);

    }
}
