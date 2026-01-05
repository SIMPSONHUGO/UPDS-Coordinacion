namespace Application.DTOs;

public class SolicitudDTO
{
    public string NombreEstudiante { get; set; } = string.Empty;
    public string Carrera { get; set; } = string.Empty;
    public string TipoSolicitud { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
}