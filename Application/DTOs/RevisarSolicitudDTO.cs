namespace Application.DTOs;

public class RevisarSolicitudDTO
{

    public int SolicitudId { get; set; } 
    public string Estado { get; set; } = string.Empty;
    public string Observacion { get; set; } = string.Empty;
}