using Application.DTOs;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases;

public class VerMisSolicitudesUseCase
{
    private readonly ISolicitudRepository _repository;

    public VerMisSolicitudesUseCase(ISolicitudRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SolicitudEstudianteDTO>> Ejecutar(int estudianteId)
    {
        var listaEntidades = await _repository.ObtenerPorEstudiante(estudianteId);

        var listaDTOs = listaEntidades.Select(s => new SolicitudEstudianteDTO
        {
            Id = s.Id,
            Motivo = s.Motivo,
            Estado = s.Estado,
            FechaSolicitud = s.FechaSolicitud,
            ObservacionJefe = s.ObservacionJefe ?? "Sin observaciones",
            RutaRespaldo = s.RutaRespaldo ?? "Sin respaldo"
        }).ToList();

        return listaDTOs;
    }
}