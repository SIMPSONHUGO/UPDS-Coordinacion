using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    // CAMBIO CLAVE: Antes era NombreCompleto, ahora es Nombre
    public string Nombre { get; set; } 

    public string Email { get; set; }

    public string Password { get; set; }

    public string Rol { get; set; } // Estudiante, Jefe, Decano

    // CAMBIO CLAVE: Antes era AreaId, ahora es CarreraId
    public int? CarreraId { get; set; } 
    
    [ForeignKey("CarreraId")]
    public virtual Carrera? Carrera { get; set; }
}