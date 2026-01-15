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
        var usuario = await _repo.ObtenerUsuarioPorEmail(dto.Email);
        if (usuario == null || usuario.Password != dto.Password)
        {
            throw new Exception("Credenciales inválidas (Usuario o Password incorrectos)");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            
            new Claim(ClaimTypes.Name, usuario.Nombre), 
            
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol) 
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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