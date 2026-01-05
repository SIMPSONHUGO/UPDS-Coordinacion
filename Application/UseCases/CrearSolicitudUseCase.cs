using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases;

public class CrearSolicitudUseCase
{
    private readonly ISolicitudRepository _repo;

    public CrearSolicitudUseCase(ISolicitudRepository repo)
    {
        _repo = repo;
    }

    public async Task Ejecutar(Solicitud solicitud)
    {
        // Regla de Negocio: Validar que tenga motivo
        if (string.IsNullOrWhiteSpace(solicitud.Motivo))
        {
            throw new Exception("El motivo es obligatorio para la licencia.");
        }

        await _repo.Crear(solicitud);
    }
}