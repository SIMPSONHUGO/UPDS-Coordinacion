namespace Application.DTOs;

public class RevisarSolicitudDTO
{
    // 🚨 Estos nombres deben coincidir con lo que escribiste en el UseCase
    public int SolicitudId { get; set; } 
    public string Estado { get; set; } = string.Empty;
    public string Observacion { get; set; } = string.Empty;
}