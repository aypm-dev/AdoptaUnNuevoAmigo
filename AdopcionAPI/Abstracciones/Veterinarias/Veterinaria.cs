using System;
using System.Collections.Generic;
using System.Text;

namespace Abstracciones.Veterinarias
{
    public  class Veterinaria
    {

        public int IdVeterinaria { get; set; }
        public DateTime FechaRegistro { get; set; } 

        public string Nombre { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public bool Activo { get; set; }


    }
}
