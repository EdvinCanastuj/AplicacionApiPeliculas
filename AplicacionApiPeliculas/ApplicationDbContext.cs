using AplicacionApiPeliculas.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AplicacionApiPeliculas
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
    }
}
