using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class SolicitudDTO
{
    public int EstudianteId { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string TipoSolicitud { get; set; } = "Licencia";
    

}