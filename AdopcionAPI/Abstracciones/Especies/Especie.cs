using Abstracciones.Razas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Abstracciones.Especies
{
    public class Especie
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEspecie { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        
        public bool Activo { get; set; }

        public DateTime FechaRegistro { get; set; }

        public List<Raza> Razas { get; set; }

    }
}
