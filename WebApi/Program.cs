using Application.Mappings;
using Application.UseCases;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. CONEXIÓN A BASE DE DATOS
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("Infrastructure")));

// 2. CONFIGURACIÓN DE CORS (Para que React se conecte)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", policy =>
    {
        policy.AllowAnyOrigin()  // En producción se recomienda poner la URL específica
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 3. INYECCIÓN DE DEPENDENCIAS (Tus servicios)
builder.Services.AddScoped<ISolicitudRepository, SolicitudRepository>();
builder.Services.AddScoped<CrearSolicitudUseCase>();
builder.Services.AddScoped<RevisarSolicitudUseCase>();
builder.Services.AddScoped<VerMisSolicitudesUseCase>();
builder.Services.AddScoped<VerReporteCoordinadorUseCase>();
builder.Services.AddScoped<LoginUseCase>();

// 4. CONFIGURACIÓN JWT (Seguridad)
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
            ValidAudience = jwtSettings.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    });

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 5. SWAGGER (Documentación)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gestion Licencias API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// --- AQUÍ EMPIEZA LA TUBERÍA (PIPELINE) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🚨 CAMBIO IMPORTANTE: CORS va primero
app.UseCors("AllowWebClient"); 

// 🚨 ESTO ES LO QUE PERMITE VER LAS FOTOS
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// TRUCO EXTRA: Asegurar que la carpeta de respaldos exista al arrancar
var carpetaRespaldos = Path.Combine(app.Environment.WebRootPath ?? "wwwroot", "respaldos");
if (!Directory.Exists(carpetaRespaldos))
{
    Directory.CreateDirectory(carpetaRespaldos);
}

app.Run();