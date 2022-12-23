//Clase que utiliza la interfaz para ocupar los metodos. (PARA CATEGORIA.)
//****EL NOMBRE DE ESTOS REPOSITORIOS SIEMPRE VAN EN PROGRAM.CS IPELICULAREPOSITORIO Y PELICULAREPOSITORIO****

using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.InterfaceRepositorio;

namespace ApiPeliculas.Repositorio
{
    //Implementamos interfaz ICategoriaRepositorio
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _bd;                       //Instancio una variable privada de ApplicationDbContext y lo instancio en una variable que se llamará _db;
        public CategoriaRepositorio(ApplicationDbContext bd)             //Crear el constructor.
        {
            _bd= bd;                                                     //le damos acceso a la DB.A través de está variable
        }


        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;                         //Llamamos al modelo categoria, fecha de creacion y la actualizamos.
            _bd.Categoria.Update(categoria);                                //Llamamos a la base de datos, tabla Categoria y actualizamos.
            return Guardar();                                               //Llamamos a la funcion Guardar() para guardar lo actualizado.
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categoria.Remove(categoria);                                //Llamamos a la Db, en Categoria y eliminamos(removemos) la categoria.
            return Guardar();                                               //Llamamos a la funcion Guardar() para guardar lo actualizado.
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categoria.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
           bool valor = _bd.Categoria.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());      //la variable valor. preguntamos a la BD en Categoria.Any cualquiera que tenga el nombre que recibo por parametros, pasada a minusculas y eliminando los espacios.
           return valor;
        }

        public bool ExisteCategoria(int id)
        {
            return _bd.Categoria.Any(c => c.Id == id);                          //En la base de datos, BUSCAMOS CUALQUIERA, DONDE CATEGORIA -> CATEGORIA.ID SEA IGUAL AL ID QUE RECIBO POR PARAMETRO.
        }

        public Categoria GetCategoria(int categoriaId)
        {
            return _bd.Categoria.FirstOrDefault(c => c.Id == categoriaId);    //Filtrar categoria por su id.
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categoria.OrderBy(c => c.Nombre).ToList();               //Listar las categorias y ordenarlas por nombre.
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;                       //Guardarmos los cambios en la BD si Guardar es mayor que cero, entonces va a retornar true, sino retorna falso.
        }
    }
}
