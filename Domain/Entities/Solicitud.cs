namespace Domain.Entities;

public class Solicitud
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    
    // El signo '?' es importante para que EF no crea que es obligatorio al crear
    public virtual Usuario? Estudiante { get; set; } 

    public string Materia { get; set; } = string.Empty;
    public string Docente { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string RutaRespaldo { get; set; } = string.Empty;
    public string Estado { get; set; } = "Pendiente";
    public string? ObservacionJefe { get; set; }
}