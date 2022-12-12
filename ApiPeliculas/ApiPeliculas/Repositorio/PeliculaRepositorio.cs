//Clase que utiliza la interfaz para ocupar los metodos. (PARA Pelicula.)

using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.InterfaceRepositorio;

namespace ApiPeliculas.Repositorio
{
    //Implementamos interfaz IPeliculaRepositorio
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _bd;                       //Instancio una variable privada de ApplicationDbContext y lo instancio en una variable que se llamará _db;
        public PeliculaRepositorio(ApplicationDbContext bd)             //Crear el constructor.
        {
            _bd= bd;                                                     //le damos acceso a la DB.A través de está variable
        }


        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;                         //Llamamos al modelo Pelicula, fecha de creacion y la actualizamos.
            _bd.Pelicula.Update(pelicula);                                //Llamamos a la base de datos, tabla Pelicula y actualizamos.
            return Guardar();                                               //Llamamos a la funcion Guardar() para guardar lo actualizado.
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Pelicula.Remove(pelicula);                                //Llamamos a la Db, en Pelicula y eliminamos(removemos) la Pelicula.
            return Guardar();                                               //Llamamos a la funcion Guardar() para guardar lo actualizado.
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Pelicula.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombre)
        {
           bool valor = _bd.Pelicula.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());      //la variable valor. preguntamos a la BD en Pelicula.Any cualquiera que tenga el nombre que recibo por parametros, pasada a minusculas y eliminando los espacios.
           return valor;
        }

        public bool ExistePelicula(int id)
        {
            return _bd.Pelicula.Any(c => c.Id == id);                          //En la base de datos, BUSCAMOS CUALQUIERA, DONDE Pelicula -> Pelicula.ID SEA IGUAL AL ID QUE RECIBO POR PARAMETRO.
        }

        public Pelicula GetPelicula(int PeliculaId)
        {
            return _bd.Pelicula.FirstOrDefault(c => c.Id == PeliculaId);    //Filtrar Pelicula por su id.
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.Pelicula.OrderBy(c => c.Nombre).ToList();               //Listar las Peliculas y ordenarlas por nombre.
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;                       //Guardarmos los cambios en la BD si Guardar es mayor que cero, entonces va a retornar true, sino retorna falso.
        }
    }
}
