using System;

namespace Application.DTOs; 

public class SolicitudDTO
{
    public int Id { get; set; }
    public string NombreEstudiante { get; set; }
    public string Carrera { get; set; } 
    public DateTime FechaSolicitud { get; set; }
    
    public string Motivo { get; set; }
    public string Materia { get; set; } 
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    public string Estado { get; set; }
    public string RutaRespaldo { get; set; }
    public string? ObservacionJefe { get; set; }
}