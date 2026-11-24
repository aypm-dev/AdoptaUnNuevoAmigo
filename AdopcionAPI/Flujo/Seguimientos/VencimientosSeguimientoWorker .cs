using Abstracciones.Seguimientos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flujo.Seguimientos
{
    public class VencimientosSeguimientoWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopes;
        private readonly ILogger<VencimientosSeguimientoWorker> _log;
        private readonly SeguimientoOpciones _op;

        public VencimientosSeguimientoWorker(IServiceScopeFactory scopes,
            ILogger<VencimientosSeguimientoWorker> log, IOptions<SeguimientoOpciones> op)
        { _scopes = scopes; _log = log; _op = op.Value; }

        protected override async Task ExecuteAsync(CancellationToken stop)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(_op.IntervaloMinutos));
            do
            {
                try
                {
                    using var scope = _scopes.CreateScope();
                    var flujo = scope.ServiceProvider.GetRequiredService<IVencimientosSeguimientoFlujo>();
                    var n = await flujo.ProcesarAsync(stop);
                    if (n > 0) _log.LogWarning("{N} seguimientos vencidos: alertas generadas.", n);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _log.LogError(ex, "Falló el ciclo de vencimientos de seguimientos.");
                }
            }
            while (await timer.WaitForNextTickAsync(stop));
        }
    }
}
