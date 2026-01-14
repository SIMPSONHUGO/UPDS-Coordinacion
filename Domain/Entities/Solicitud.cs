using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Solicitud
{
    [Key]
    public int Id { get; set; } // 🚨 CAMBIO IMPORTANTE: De Guid a int

    public int EstudianteId { get; set; } // 🚨 Asegúrate que este también sea int
    
    [ForeignKey("EstudianteId")]
    public Usuario Estudiante { get; set; }

    public string Motivo { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public string TipoSolicitud { get; set; } = "Licencia";
    public string? RutaRespaldo { get; set; }
    public string? ObservacionJefe { get; set; }
}