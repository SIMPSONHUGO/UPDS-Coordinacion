using Domain.Entities;

namespace Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {

        context.Database.EnsureCreated();


        if (context.Usuarios.Any()) return;

        var usuarios = new Usuario[]
        {

            new Usuario { Nombre = "Vanessa Illonka", Email = "vanessa@upds.edu.bo", Password = "123", Rol = "Jefe", Area = "Social", Carrera = "N/A" },

            // Área Empresarial
            new Usuario { Nombre = "Javier Lopez", Email = "javier@upds.edu.bo", Password = "123", Rol = "Jefe", Area = "Empresarial", Carrera = "N/A" },

            // Área Derecho
            new Usuario { Nombre = "Ana Olgin", Email = "ana@upds.edu.bo", Password = "123", Rol = "Jefe", Area = "Derecho", Carrera = "N/A" },

            // Área Sistemas y Telecomunicaciones
            new Usuario { Nombre = "Eiver Velasquez", Email = "eiver@upds.edu.bo", Password = "123", Rol = "Jefe", Area = "Sistemas", Carrera = "N/A" },

            // Área Civil, Industrial, Petrolera
            new Usuario { Nombre = "Martin del Rio", Email = "martin@upds.edu.bo", Password = "123", Rol = "Jefe", Area = "Industrial", Carrera = "N/A" },


            //DECANAS 
            new Usuario { Nombre = "Elena Ponce", Email = "elena@upds.edu.bo", Password = "123", Rol = "Decano", Area = "Ingeniería", Carrera = "N/A" },
            new Usuario { Nombre = "Andrea Flores", Email = "andrea@upds.edu.bo", Password = "123", Rol = "Decano", Area = "Empresarial", Carrera = "N/A" },
            new Usuario { Nombre = "Noemi Calizaya", Email = "noemi@upds.edu.bo", Password = "123", Rol = "Decano", Area = "Social", Carrera = "N/A" },

            //COORDINADOR ACADÉMICO
            new Usuario { Nombre = "Coordinador General", Email = "coord@upds.edu.bo", Password = "123", Rol = "Coordinador", Area = "Académica", Carrera = "N/A" }
        };

        context.Usuarios.AddRange(usuarios);
        context.SaveChanges();
    }
}