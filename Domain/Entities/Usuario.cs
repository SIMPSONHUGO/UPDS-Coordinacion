namespace Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Rol { get; set; }
    public string? Carrera { get; set; }
    public string? Area { get; set; }   
}