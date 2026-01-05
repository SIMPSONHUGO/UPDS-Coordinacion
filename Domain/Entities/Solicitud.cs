using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Solicitud
{
    public Guid Id { get; set; }

    // Datos del Estudiante
    public string NombreCompleto { get; set; } = string.Empty;
    public string Carrera { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;

    // Datos de la Solicitud
    public string TipoSolicitud { get; set; } = string.Empty; 
    public string Motivo { get; set; } = string.Empty; 
    
    // Ruta del archivo adjunto
    public string RutaRespaldo { get; set; } = string.Empty; 

    // Estado inicial
    public string Estado { get; set; } = "Pendiente"; 
    
    public DateTime FechaSolicitud { get; set; } = DateTime.Now;
}