using Microsoft.AspNetCore.Http;
using System;

namespace Application.DTOs;

// CLAVE: El nombre debe ser SolicitudCreacionDTO
public class SolicitudCreacionDTO
{
    public string Motivo { get; set; } = string.Empty;
    public string Materia { get; set; } = string.Empty;
    public string Docente { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    // Este campo es el que recibe la foto desde React
    public IFormFile? Archivo { get; set; } 
}