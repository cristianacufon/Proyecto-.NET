using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.InterfaceRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    //[Authorize]
    //[Authorize(Roles = "admin")]  Asi se puede agregar que solo un tipo de usuario pueda acceder. Y si va aquí es a nivel de clase... tambien se puede hacer a nivel de Método.
    //[Route("api/[controller]")]      ---- ESTA RUTA ES PARA EL CONTROLADOR DINAMICO, OSEA QUE SI CAMBIO EL NOMBRE DEL CategoriasController, cambiaria el nombre del [controller]. a diferencia del de abajo que es estatico y si cambia el nombre no se perderia la api creada.
    [ApiController]
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        //CONSTRUCTOR.
        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        [AllowAnonymous] //Para que puedan ver sin login el tipo de categorias.
        [HttpGet]
        //[ResponseCache(Duration = 20)]     //Caché con duracion de 20 segundos
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] / En este caso no habrá caché (ResponseCacheLocation.None), por si necesito varias llamadas al servidor, con NoStore no se guardan los errores constantemente. 
        [ResponseCache(CacheProfileName = "PorDefecto20Segundos")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctRepo.GetCategorias();

            var listaCategoriaDto = new List<CategoriaDto>();
            //A la variable lista, se le pasará todo lo que está en listaCategorias. Despues en listaCategoriaDto se le añadirá todo lo que se obtuvo en la lista anterior (lista).
            foreach (var lista in listaCategorias)
            {
                listaCategoriaDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(listaCategoriaDto);
        }

        [AllowAnonymous] //Para que puedan ver sin login el tipo de categorias.
        [HttpGet("{categoriaId:int})", Name = "GetCategoria")]
        [ResponseCache(CacheProfileName = "PorDefecto20Segundos")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCategoria = _ctRepo.GetCategoria(categoriaId);

            if (itemCategoria == null)
            {
                return NotFound();
            }

            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
            return Ok(itemCategoriaDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        //vamos a utilizar el metodo que viene del repositorio (CrearCategoria).el cual (CrearCategoria) va a utilizar el modelo Categoria.
        // Primero indicamos de donde lo va a obtener, como esta en formato JSON hay que utilizar FromBody
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            //ModelState verifica que el modelo no este erroneo, es decir que se cumplan las condiciones que tiene el modelo, en este caso que el nombre no sea nulo y no tenga mas de 60 caracteres.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }

            //Entramos al _ctRepo para entrar a la BD y ver si Existe la categoria pasandole el parametro crearCategoriaDto, por ejemplo ver si ya existe una categoria llamada accion
            if (_ctRepo.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }
            //En la variable categiria se muestra si existe o no ese nombre de categoria.
            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);
            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            //Entonces si esta bueno.
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }

        [Authorize(Roles = "admin")]
        //Metodo patch, actualizar algunos campos a diferencia de PUT.
        [HttpPatch("{categoriaId:int}", Name = "ActualizarPatchCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDto CategoriaDto)
        {
            //ModelState verifica que el modelo no este erroneo, es decir que se cumplan las condiciones que tiene el modelo, en este caso que el nombre no sea nulo y no tenga mas de 60 caracteres.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CategoriaDto == null || categoriaId != CategoriaDto.Id)
            {
                return BadRequest(ModelState);
            }

            //MAPEAR LA CATEGORIA DE CATEGORIA A CATEGORIADTO
            var categoria = _mapper.Map<Categoria>(CategoriaDto);

            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            //Entonces si esta bueno.
            return NoContent();

        }

        [Authorize(Roles = "admin")]
        //METODO DELETE. 
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult BorrarCategoria(int categoriaId)
        {
               
            if (!_ctRepo.ExisteCategoria(categoriaId))
            {
                return NotFound();
            }
            //MAPEAR LA CATEGORIA DE CATEGORIA A CATEGORIADTO
            var categoria = _ctRepo.GetCategoria(categoriaId);

            if (!_ctRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"No existe el registro{categoria.Nombre} para eliminarlo");
                return StatusCode(500, ModelState);
            }

            //Entonces si esta bueno.
            return NoContent();
        }
    }
}
