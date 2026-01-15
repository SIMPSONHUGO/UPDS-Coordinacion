using Application.DTOs;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases;

public class RevisarSolicitudUseCase
{
    private readonly ISolicitudRepository _repository;

    public RevisarSolicitudUseCase(ISolicitudRepository repository)
    {
        _repository = repository;
    }

    public async Task Ejecutar(RevisarSolicitudDTO dto)
    {

        var solicitud = await _repository.ObtenerPorId(dto.SolicitudId);

        if (solicitud == null)
        {
            throw new Exception("Solicitud no encontrada");
        }


        solicitud.Estado = dto.Estado;
        solicitud.ObservacionJefe = dto.Observacion;

        await _repository.Actualizar(solicitud);
    }
}