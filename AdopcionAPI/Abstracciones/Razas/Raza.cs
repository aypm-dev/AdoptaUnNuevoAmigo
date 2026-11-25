using Abstracciones.Especies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abstracciones.Razas
{
    public class Raza
    {

        public int IdRaza { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int EspecieId { get; set; }
        public Especie Especie { get; set; }

        public List<Abstracciones.Animales.Animal> Animales { get; set; }

    }
}
