using System.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.InterfaceRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    //[Route("api/[controller]")]  ESTO ES LO QUE SE CREA, PERO ES MEJOR DEJARLO ESTATICO (PELICULA) POR SI UN DIA CAMBIA LA RUTA Y NO SE PIERDA EL ACCESO.
    [Route("api/Peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;

        //CONSTRUCTOR.
        public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
        }
        //ESTO ES EL ENDPOINT
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _pelRepo.GetPeliculas();

            var listaPeliculaDto = new List<PeliculaDto>();
            //A la variable lista, se le pasará todo lo que está en listaCategorias. Despues en listaCategoriaDto se le añadirá todo lo que se obtuvo en la lista anterior (lista).
            foreach (var lista in listaPeliculas)
            {
                listaPeliculaDto.Add(_mapper.Map<PeliculaDto>(lista));
            }
            return Ok(listaPeliculaDto);
        }

        [AllowAnonymous]
        [HttpGet("{peliculaId:int})", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _pelRepo.GetPelicula(peliculaId);

            if (itemPelicula == null)
            {
                return NotFound();
            }

            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);
            return Ok(itemPeliculaDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //vamos a utilizar el metodo que viene del repositorio (CrearCategoria).el cual (CrearCategoria) va a utilizar el modelo Categoria.
        // Primero indicamos de donde lo va a obtener, como esta en formato JSON hay que utilizar FromBody
        public IActionResult CrearPelicula([FromBody] PeliculaDto peliculaDto)
        {
            //ModelState verifica que el modelo no este erroneo, es decir que se cumplan las condiciones que tiene el modelo, en este caso que el nombre no sea nulo y no tenga mas de 60 caracteres.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            //Entramos al _ctRepo para entrar a la BD y ver si Existe la categoria pasandole el parametro crearCategoriaDto, por ejemplo ver si ya existe una categoria llamada accion
            if (_pelRepo.ExistePelicula(peliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }
            //En la variable categiria se muestra si existe o no ese nombre de categoria.
            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_pelRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            //Entonces si esta bueno. RETORNAR EL ID DE LA PELICULA QUE SI SE CREÓ. 
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }

        [Authorize(Roles = "admin")]
        //Metodo patch, actualizar algunos campos a diferencia de PUT.
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            //ModelState verifica que el modelo no este erroneo, es decir que se cumplan las condiciones que tiene el modelo, en este caso que el nombre no sea nulo y no tenga mas de 60 caracteres.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //MAPEAR LA CATEGORIA DE CATEGORIA A CATEGORIADTO
            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            //Entonces si esta bueno.
            return NoContent();

        }

        [Authorize(Roles = "admin")]
        //METODO DELETE. 
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult BorrarPelicula(int peliculaId)
        {

            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }
            //Vamos a obtener la pelicula por el ID introducido.
            var pelicula = _pelRepo.GetPelicula(peliculaId);
            //Si no se pudo borrar la pelicula, mandamos el mensaje.
            if (!_pelRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"No existe el registro{pelicula.Nombre} para eliminarlo");
                return StatusCode(500, ModelState);
            }

            //Entonces si esta bueno.
            return NoContent();
        }


        //Para filtrar pelicula por categoria. Se pasará por parametro el ID de la categoria.
        [AllowAnonymous]
        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);
            
            if (listaPeliculas == null)
            {
                return NotFound();
            }

            var itemPelicula = new List<PeliculaDto>();
            foreach (var item in listaPeliculas)
            {
                itemPelicula.Add(_mapper.Map<PeliculaDto>(item));
            }

            return Ok(itemPelicula );
        }

        //Para filtrar pelicula por categoria. Se pasará por parametro el ID de la categoria.
        [AllowAnonymous]
        [HttpGet("Buscar")]
        public IActionResult Buscar(string nombre)
        {
            var resultado = _pelRepo.BuscarPelicula(nombre.Trim());
            try
            {
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
          
        }
    }
}
