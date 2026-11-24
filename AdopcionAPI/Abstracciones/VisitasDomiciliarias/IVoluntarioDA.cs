using System;
using System.Collections.Generic;
using System.Text;

namespace Abstracciones.VisitasDomiciliarias
{
    public interface IVoluntarioDA
    {

        Task<bool> ExisteActivoAsync(int id, CancellationToken ct);

    }
}
