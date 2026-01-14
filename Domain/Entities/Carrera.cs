using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Carrera
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;
}