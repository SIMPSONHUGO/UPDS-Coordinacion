using System;

namespace Application.DTOs;

public class SolicitudCoordinadorDTO
{
    public int Id { get; set; }
    
    // Inicializamos todo con string.Empty para evitar warnings
    public string NombreEstudiante { get; set; } = string.Empty;
    public string Carrera { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    
    // Cambiamos a string para formatear fecha (o déjalo DateTime si prefieres)
    public string FechaSolicitud { get; set; } = string.Empty; 
    
    public string Estado { get; set; } = string.Empty;
    public string ObservacionJefe { get; set; } = "Sin observaciones";
    
    // Nueva propiedad para ver si el Coordinador puede ver la foto
    public string RutaRespaldo { get; set; } = "Sin respaldo";
}