using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface ISolicitudRepository
{

    Task Crear(Solicitud solicitud);
    Task Actualizar(Solicitud solicitud);
    
    Task<Solicitud?> ObtenerPorId(int id);

    Task<List<Solicitud>> ObtenerTodas();
    Task<List<Solicitud>> ObtenerPorEstudiante(int estudianteId);

    Task<Usuario?> ObtenerUsuarioPorEmail(string email);
    Task<Usuario?> ObtenerUsuarioPorId(int id);
}