using Domain.Entities;

namespace Domain.Interfaces;

public interface ISolicitudRepository
{
    // Definimos el contrato: Guardar y Leer
    Task Crear(Solicitud solicitud);
    Task<IEnumerable<Solicitud>> ObtenerTodas();
}