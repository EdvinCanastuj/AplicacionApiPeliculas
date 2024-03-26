
using AplicacionApiPeliculas;
using AplicacionApiPeliculas.DTO;
using AplicacionApiPeliculas.Entidades;
using AplicacionApiPeliculas.Filtros;
using AplicacionApiPeliculas.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]
    [ApiController]

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController: ControllerBase
    {

        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController( ILogger<GenerosController> logger,
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable =  context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (genero == null)
            {
                return NotFound();
            }
            return mapper.Map<GeneroDTO>(genero);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var generoNuevo = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(generoNuevo);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        { 
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (genero == null)
            {
                return NotFound();
            }
            genero = mapper.Map(generoCreacionDTO, genero);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Genero() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
