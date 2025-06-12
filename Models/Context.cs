using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace PNT1_TP_Cine.Models
{
    public class Context : DbContext
    {
      

        public DbSet<Funcion> Funciones { get; set; } = null!;
        public DbSet<Pelicula> Peliculas { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<Genero> Generos { get; set; } = null!;
        public DbSet<Sala> Salas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=PNT1_TP1_Cine;" +
                           " Integrated Security= true; TrustServerCertificate= true; Encrypt= true");
            base.OnConfiguring(optionsBuilder);

        }
    }
}
