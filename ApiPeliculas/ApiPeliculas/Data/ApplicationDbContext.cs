using ApiPeliculas.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    //public class ApplicationDbContext : DbContext    Implementando Identity ya no es necesario ocupar DbContext, ahora se debe llamar a IdentityDbContext con el modelo<Appusuario>
    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {   //Instanciar la BD (?)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)  : base(options)
        {
        }

        //Utilizar este metodo para el modelo AppUsuario,    PARA QUE SE DETECTE EL IDENTITY CUANDO SE HAGA LA MIGRACIÓN.    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 
        }

        //Agregar los modelos aqui
        public DbSet<Categoria> Categoria { get; set; } 
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        //Agregamos el nuevo modelo que ahora es AppUsuario. 
        public DbSet<AppUsuario> AppUsuario { get; set; }

    }
}
