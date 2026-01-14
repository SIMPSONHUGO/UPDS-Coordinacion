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
        // Buscamos la solicitud por su ID (número entero)
        var solicitud = await _repository.ObtenerPorId(dto.SolicitudId);

        if (solicitud == null)
        {
            throw new Exception("Solicitud no encontrada");
        }

        // Actualizamos los datos
        solicitud.Estado = dto.Estado; // "Aprobada" o "Rechazada"
        solicitud.ObservacionJefe = dto.Observacion;

        // Guardamos cambios
        await _repository.Actualizar(solicitud);
    }
}