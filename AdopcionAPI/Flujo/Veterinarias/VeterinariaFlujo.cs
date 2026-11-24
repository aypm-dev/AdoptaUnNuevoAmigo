using Abstracciones.Comunes;
using Abstracciones.Especies;
using Abstracciones.Excepciones;
using Abstracciones.Veterinarias;
using DA;
using Flujo.Especies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flujo.Veterinarias
{
    public class VeterinariaFlujo : IVeterinariaFlujo
    {

        private readonly IVeterinariaDA _veterinariaDA;
        private readonly TimeProvider _reloj;

        public VeterinariaFlujo(IVeterinariaDA veterinariaDA, TimeProvider reloj)
        {
            _veterinariaDA = veterinariaDA;
            _reloj = reloj;
        }

        public async Task ActualizarAsync(int id, VeterinariaDtos.VeterinariaActualizarDTO dto, CancellationToken ct)
        {
            _ = await _veterinariaDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Veterinaria", id);

            // La descripción puede repetirse solo si pertenece a la MISMA veterinaria que se está editando.
            var actualizado = await _veterinariaDA.ActualizarAsync(new Veterinaria
            {
                IdVeterinaria = id,
                Nombre = dto.Nombre.Trim(),
                FechaRegistro = dto.FechaRegistro,
                Diagnostico = dto.Diagnostico.Trim(),
                Tratamiento = dto.Tratamiento.Trim(),
                Activo = dto.Activo
            }, ct);

            if (!actualizado) throw new NoEncontradoException("Veterinaria", id);
        }

        public async Task<VeterinariaDtos.VeterinariaDto> CrearAsync(VeterinariaDtos.VeterinariaCrearDTO dto, CancellationToken ct)
        {
            var veterinaria = new Veterinaria
            {
                Nombre = dto.Nombre.Trim(),
                FechaRegistro = dto.FechaRegistro,
                Diagnostico = dto.Diagnostico.Trim(),
                Tratamiento = dto.Tratamiento.Trim(),
                Activo = dto.Activo
            };

            await _veterinariaDA.AgregarAsync(veterinaria, ct);
            return VeterinariaMapeador.ADto(veterinaria);
        }

        public async Task EliminarAsync(int id, CancellationToken ct)
        {
            _ = await _veterinariaDA.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Veterinaria", id);

            if (!await _veterinariaDA.EliminarAsync(id, ct))
                throw new NoEncontradoException("Veterinaria", id);
        }

        public async Task<ResultadoPaginado<VeterinariaDtos.VeterinariaDto>> ListarAsync(VeterinariaDtos.VeterinariaFiltro filtro, CancellationToken ct)
        {
            var pagina = await _veterinariaDA.BuscarAsync(filtro, ct);
            return new ResultadoPaginado<VeterinariaDtos.VeterinariaDto>
            {
                Items = pagina.Items.Select(VeterinariaMapeador.ADto).ToList(),
                Total = pagina.Total,
                Pagina = pagina.Pagina,
                Tamano = pagina.Tamano
            };
        }

        public async Task<VeterinariaDtos.VeterinariaDto> ObtenerAsync(int id, CancellationToken ct)
        {
            var veterinaria = await _veterinariaDA.ObtenerPorIdAsync(id, ct)
            ?? throw new NoEncontradoException("Veterinaria", id);
            return VeterinariaMapeador.ADto(veterinaria);
        }
    }
}
