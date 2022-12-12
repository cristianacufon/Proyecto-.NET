using ApiPeliculas.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {   //Instanciar la BD (?)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)  : base(options)
        {
        }

        //Agregar los modelos aqui
        public DbSet<Categoria> Categoria { get; set; } 
        public DbSet<Pelicula> Pelicula { get; set; }
    }
}
