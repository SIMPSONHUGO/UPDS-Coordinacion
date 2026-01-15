using Application.DTOs;
using Application.UseCases;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs; 

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly AppDbContext _context;

    public AuthController(LoginUseCase loginUseCase, AppDbContext context)
    {
        _loginUseCase = loginUseCase;
        _context = context;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try
        {
            string token = await _loginUseCase.Ejecutar(dto);
            return Ok(new { token = token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistroEstudianteDTO request)
    {
        try 
        {
            bool existe = await _context.Usuarios.AnyAsync(u => u.Email == request.Email);
            if (existe)
            {
                return BadRequest("El correo electrónico ya está registrado.");
            }

            var nuevoUsuario = new Usuario
            {
                Nombre = request.NombreCompleto,
                Email = request.Email,
                Password = request.Password, 
                Rol = "Estudiante",
                Carrera = request.Carrera,
                Area = "Estudiante"
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registro exitoso. Ahora puedes iniciar sesión." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "No se pudo registrar el usuario: " + ex.Message });
        }
    }
}