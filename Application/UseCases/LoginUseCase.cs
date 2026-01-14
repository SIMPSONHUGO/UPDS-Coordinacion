using Application.DTOs;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.UseCases;

public class LoginUseCase
{
    private readonly ISolicitudRepository _repo;
    private readonly IConfiguration _config; 

    public LoginUseCase(ISolicitudRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task<string> Ejecutar(LoginDTO dto)
    {
        // 1. Buscamos al usuario por Email
        var usuario = await _repo.ObtenerUsuarioPorEmail(dto.Email);

        // 2. Validamos si existe y si la contraseña coincide
        if (usuario == null || usuario.Password != dto.Password)
        {
            throw new Exception("Credenciales inválidas (Usuario o Password incorrectos)");
        }

        // 3. Crear los "Permisos" (Claims) que irán dentro del Token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            
            // ⚠️ CORRECCIÓN AQUÍ: Antes era .NombreCompleto, ahora usamos .Nombre
            new Claim(ClaimTypes.Name, usuario.Nombre), 
            
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol) 
        };

        // 4. Crear la Llave de Seguridad
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 5. Fabricar el Token
        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1), 
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}