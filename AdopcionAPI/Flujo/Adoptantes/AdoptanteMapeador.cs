using Abstracciones.Adoptantes;
using Abstracciones.Animales;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Adoptantes.AdoptanteDtos;
using static Abstracciones.Animales.AnimalDtos;

namespace Flujo.Adoptantes
{
    public class AdoptanteMapeador
    {

        public static AdoptanteDto ADto(Adoptante a) => new(
        a.IdAdoptante,
        a.Identificacion, 
        a.Nombre,
        a.Contacto,
        a.OtrasMascotas,
        a.TipoViviendaId,
        a.TipoVivienda?.Nombre,
        a.FechaRegistro,
        a.Activo);

    }
}
