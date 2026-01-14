using Microsoft.AspNetCore.Http; // Necesario para IFormFile

namespace WebApi.DTOs;

public class SolicitudCreacionDTO
{
    public int EstudianteId { get; set; }
    public string TipoSolicitud { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    
    // ESTA ES LA CLAVE: El archivo real que viene del navegador
    public IFormFile? Archivo { get; set; } 
}