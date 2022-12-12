using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.InterfaceRepositorio
{
    public interface ICategoriaRepositorio
    {
        ICollection<Categoria>GetCategorias(); //ICollection de Modelo categoria, obtener categorias... ESTE ES EL METODO QUE NOS TRAERA LAS CATEGORIAS.
        Categoria GetCategoria(int categoriaId); //Instanciamos el metodo categoria y va a recibir el Id de la categoria 
        bool ExisteCategoria(string nombre);   //este metodo buleano va a consultar si existe la categoria por su nombre
        bool ExisteCategoria(int id); //este metodo buleano va a consultar si existe la categoria por su Id
        bool CrearCategoria(Categoria categoria); //este metodo buleano va a crear una categoria recibiendo el modelo Categoria.
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        bool Guardar();
    }
}
