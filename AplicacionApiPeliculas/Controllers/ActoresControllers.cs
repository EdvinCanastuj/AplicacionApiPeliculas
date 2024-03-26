

using AplicacionApiPeliculas.DTO;
using AplicacionApiPeliculas.Entidades;
using AplicacionApiPeliculas.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplicacionApiPeliculas.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresControllers : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private string contenedor = "actores";
        public ActoresControllers(ApplicationDbContext context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }
        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
               var queryable = context.Actores.AsQueryable();
                await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
                var actores = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
                return mapper.Map<List<ActorDTO>>(actores);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            return mapper.Map<ActorDTO>(actor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            if (actorCreacionDTO.Foto != null)
            {
                actor.Foto = await almacenadorArchivos.GuardarArchivo(contenedor, actorCreacionDTO.Foto);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actorDB == null)
            {
                return NotFound();
            }
            actorDB = mapper.Map(actorCreacionDTO, actorDB);
            if (actorCreacionDTO.Foto != null)
            {
                actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenedor, actorCreacionDTO.Foto, actorDB.Foto);
            }
            await context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            context.Remove(actor);
            await context.SaveChangesAsync();
            await almacenadorArchivos.BorrarArchivo(actor.Foto, contenedor);
            return NoContent();
        }

    }
}
