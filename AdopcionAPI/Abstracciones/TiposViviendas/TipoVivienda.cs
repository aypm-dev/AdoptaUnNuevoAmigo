using Abstracciones.Adoptantes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Abstracciones.TiposViviendas
{
    public class TipoVivienda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_TIPO_VIVIENDA")]
        public int IdTipoVivienda { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        public bool Activo { get; set; }

        public  DateTime FechaRegistro { get; set; }

        public List<Adoptante> Adoptantes { get; set; } 

    }
}
