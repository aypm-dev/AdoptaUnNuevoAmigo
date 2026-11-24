using Abstracciones.Animales;
using Abstracciones.Veterinarias;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abstracciones.Consultas
{
    public class ConsultaVeterinaria
    {

        public int IdConsulta { get; set; }
        public int VeterinariaId { get; set; }
        public int AnimalId { get; set; }
        public DateTime Fecha { get; set; }
        public string Diagnostico { get; set; } = null!;
        public string Tratamiento { get; set; } = null!;

        public Veterinaria? Veterinaria { get; set; }
        public Animal? Animal { get; set; }

    }



}
