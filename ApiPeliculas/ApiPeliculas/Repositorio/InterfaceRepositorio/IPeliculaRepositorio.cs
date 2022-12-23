using ApiPeliculas.Modelos;
//****EL NOMBRE DE ESTOS REPOSITORIOS SIEMPRE VAN EN PROGRAM.CS IPELICULAREPOSITORIO Y PELICULAREPOSITORIO****
namespace ApiPeliculas.Repositorio.InterfaceRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Pelicula>GetPeliculas(); //ICollection de Modelo Pelicula, obtener Peliculas... ESTE ES EL METODO QUE NOS TRAERA LAS PeliculaS.
        Pelicula GetPelicula(int peliculaId); //Instanciamos el metodo Pelicula y va a recibir el Id de la Pelicula 
        bool ExistePelicula(string nombre);   //este metodo buleano va a consultar si existe la Pelicula por su nombre
        bool ExistePelicula(int id); //este metodo buleano va a consultar si existe la Pelicula por su Id
        bool CrearPelicula(Pelicula pelicula); //este metodo buleano va a crear una Pelicula recibiendo el modelo Pelicula.
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);

        //Métodos para buscar peliculas en categoria y buscar pelicula por nombre.
        ICollection<Pelicula> GetPeliculasEnCategoria(int catId);
        ICollection<Pelicula> BuscarPelicula(string nombre);
        bool Guardar();
    }
}
