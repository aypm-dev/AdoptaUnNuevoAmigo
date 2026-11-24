using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Web.Clients
{
    public class EtiquetasApiClient
    {

        private readonly HttpClient _http;

        public EtiquetasApiClient(HttpClient http) => _http = http;

        public async Task<List<EtiquetaDto>> ListarAsync(CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<List<EtiquetaDto>>("api/etiquetas", ct)
                   ?? new List<EtiquetaDto>();
        }

        public async Task<EtiquetaDto?> ObtenerAsync(int id, CancellationToken ct = default)
        {
            var resp = await _http.GetAsync($"api/etiquetas/{id}", ct);
            if (resp.StatusCode == HttpStatusCode.NotFound)
                return null;

            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<EtiquetaDto>(ct);
        }

        public async Task<(bool Ok, EtiquetaDto? Creada, string? Error)> CrearAsync(
            EtiquetaCrearDto dto, CancellationToken ct = default)
        {
            var resp = await _http.PostAsJsonAsync("api/etiquetas", dto, ct);

            if (resp.IsSuccessStatusCode)
            {
                var creada = await resp.Content.ReadFromJsonAsync<EtiquetaDto>(ct);
                return (true, creada, null);
            }

            if (resp.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Conflict)
            {
                var problem = await resp.Content.ReadFromJsonAsync<ProblemDetails>(ct);
                var mensaje = problem?.Detail ?? problem?.Title ?? "No se pudo crear la etiqueta.";
                return (false, null, mensaje);
            }

            resp.EnsureSuccessStatusCode();
            return (false, null, "Error inesperado al comunicarse con el API.");
        }

    }
}
