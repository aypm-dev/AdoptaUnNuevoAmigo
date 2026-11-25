using Abstracciones.Animales;
using Abstracciones.Razas;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Animales.AnimalDtos;
using static Abstracciones.Razas.RazaDtos;

namespace Flujo.Animales
{
    internal static class AnimalMapeador
    {

        public static AnimalDto ADto(Animal a) => new(
         a.IdAnimal,
         a.Nombre,
         a.RazaId,
         a.FechaNacimiento,
         a.Sexo,
         a.Tamanio,
         a.Esterilizado,
         a.Raza?.Nombre,
         a.Activo,
         a.Estado);

    }
}
