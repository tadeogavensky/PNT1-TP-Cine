using Microsoft.EntityFrameworkCore;

namespace PNT1_TP_Cine.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Funcion> Funciones { get; set; } = null!;
        public DbSet<Pelicula> Peliculas { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<Genero> Generos { get; set; } = null!;
        public DbSet<Sala> Salas { get; set; } = null!;
    }
}
