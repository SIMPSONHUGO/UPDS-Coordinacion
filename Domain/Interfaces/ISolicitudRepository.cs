using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface ISolicitudRepository
{
    // Métodos de escritura
    Task Crear(Solicitud solicitud);
    Task Actualizar(Solicitud solicitud);
    
    // 🚨 CORRECCIÓN 1: Cambiamos 'Guid' por 'int' (porque tus IDs son números)
    Task<Solicitud?> ObtenerPorId(int id);

    // 🚨 CORRECCIÓN 2: Usamos el nombre estándar que pusimos en el Repositorio
    Task<List<Solicitud>> ObtenerTodas();
    
    // 🚨 CORRECCIÓN 3: Le quitamos el "Id" al final para que coincida con tu UseCase
    Task<List<Solicitud>> ObtenerPorEstudiante(int estudianteId);
    
    // Métodos de usuario (los mantenemos si los estabas usando)
    Task<Usuario?> ObtenerUsuarioPorEmail(string email);
    Task<Usuario?> ObtenerUsuarioPorId(int id);
}