using Abstracciones.Animales;
using Abstracciones.TiposVacunas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abstracciones.Vacunas
{
    public class Vacunacion
    {

        public int IdVacunacion { get; set; }
        public int AnimalId { get; set; }
        public int TipoVacunaId { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime ProximaDosis { get; set; }

        public Animal? Animal { get; set; }
        public TipoVacuna? TipoVacuna { get; set; }
        

    }
}
