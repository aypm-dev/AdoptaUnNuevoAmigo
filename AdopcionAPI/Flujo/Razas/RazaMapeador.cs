using Abstracciones.Razas;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static Abstracciones.Razas.RazaDtos;

namespace Flujo.Razas
{
    internal static class RazaMapeador
    {

        public static RazaDto ADto(Raza r) => new(
      r.IdRaza,
      r.Nombre,
      r.Descripcion,
      r.EspecieId,
      r.Especie?.Nombre,
      r.FechaRegistro);

    }
}
