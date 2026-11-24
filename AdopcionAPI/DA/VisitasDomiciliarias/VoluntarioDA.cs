using Abstracciones.VisitasDomiciliarias;
using DA.Contexto.AdopcionContexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.VisitasDomiciliarias
{
    public class VoluntarioDA : IVoluntarioDA
    {

        private readonly AdopcionContexto _ctx;
        public VoluntarioDA(AdopcionContexto ctx) => _ctx = ctx;
        public Task<bool> ExisteActivoAsync(int id, CancellationToken ct) =>
            _ctx.Voluntarios.AnyAsync(v => v.IdVoluntario == id && v.Activo, ct);


    }
}
