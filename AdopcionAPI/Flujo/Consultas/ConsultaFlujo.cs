using Abstracciones.Animales;
using Abstracciones.Comunes;
using Abstracciones.Consultas;
using Abstracciones.Excepciones;
using Abstracciones.Veterinarias;
using System;
using System.Collections.Generic;
using System.Text;
using static Abstracciones.Consultas.ConsultaDtos;

namespace Flujo.Consultas
{
    public class ConsultaFlujo: IConsultaFlujo
    {

        private readonly IConsultaDA _da;
        private readonly IAnimalDA _animalDA;
        private readonly IVeterinariaDA _veterinarioDA;
        private readonly TimeProvider _reloj;

        public ConsultaFlujo(IConsultaDA da, IAnimalDA animalDA, IVeterinariaDA veterinarioDA, TimeProvider reloj)
        { _da = da; _animalDA = animalDA; _veterinarioDA = veterinarioDA; _reloj = reloj; }

        public async Task<ConsultaDto> CrearAsync(ConsultaCrearDTO dto, CancellationToken ct)
        {
            var animal = await _animalDA.ObtenerPorIdAsync(dto.AnimalId, ct) ?? throw new NoEncontradoException("Animal", dto.AnimalId);
            if (!animal.Activo) throw new ValidacionNegocioException("El animal está inactivo.");
            _ = await _veterinarioDA.ObtenerPorIdAsync(dto.VeterinariaId, ct) ?? throw new NoEncontradoException("Veterinaria", dto.VeterinariaId);
            ValidarFecha(dto.Fecha);

            var c = await _da.AgregarAsync(new ConsultaVeterinaria
            {
                VeterinariaId = dto.VeterinariaId,
                AnimalId = dto.AnimalId,
                Fecha = dto.Fecha,
                Diagnostico = dto.Diagnostico.Trim(),
                Tratamiento = dto.Tratamiento.Trim()
            }, ct);
            return await ObtenerAsync(c.IdConsulta, ct);
        }

        public async Task ActualizarAsync(int id, ConsultaActualizarDTO dto, CancellationToken ct)
        {
            _ = await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Consulta", id);
            ValidarFecha(dto.Fecha);

            if (!await _da.ActualizarAsync(new ConsultaVeterinaria
            { IdConsulta = id, Fecha = dto.Fecha, Diagnostico = dto.Diagnostico.Trim(), Tratamiento = dto.Tratamiento.Trim() }, ct))
                throw new NoEncontradoException("Consulta", id);
        }

        private void ValidarFecha(DateTime fecha)
        {
            if (fecha > _reloj.GetUtcNow().UtcDateTime)
                throw new ValidacionNegocioException("La fecha de la consulta no puede estar en el futuro.");
        }

        public async Task<ResultadoPaginado<ConsultaDto>> ListarAsync(ConsultaFiltro f, CancellationToken ct)
        {
            var p = await _da.BuscarAsync(f, ct);
            return new ResultadoPaginado<ConsultaDto>
            { Items = p.Items.Select(ConsultaMapeador.ADto).ToList(), Total = p.Total, Pagina = p.Pagina, Tamano = p.Tamano };
        }

        public async Task<ConsultaDto> ObtenerAsync(int id, CancellationToken ct) =>
            ConsultaMapeador.ADto(await _da.ObtenerPorIdAsync(id, ct) ?? throw new NoEncontradoException("Consulta", id));

    }
}
