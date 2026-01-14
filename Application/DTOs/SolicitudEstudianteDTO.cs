using System;

namespace Application.DTOs;

public class SolicitudEstudianteDTO
{
    public int Id { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }

    public string ObservacionJefe { get; set; } = "Sin observaciones";
    public string RutaRespaldo { get; set; } = "Sin respaldo";
}