using DirectorioAPI.Services;
using DirectorioCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace DirectorioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : Controller
    {
        private readonly DirectorioService _directorioService;
        private readonly ILogger<PersonasController> _logger;

        public PersonasController(DirectorioService directorioService, ILogger<PersonasController> logger)
        {
            _directorioService = directorioService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> Get()
        {
            var personas = await _directorioService.ObtenerTodasPersonas();
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> Get(int id)
        {
            var persona = await _directorioService.ObtenerPersonaPorId(id);
            if (persona == null) return NotFound();
            return Ok(persona);
        }

        [HttpPost]
        public async Task<ActionResult<Persona>> Post(Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevaPersona = await _directorioService.AlmacenarPersona(persona);
            return CreatedAtAction(nameof(Get), new { id = nuevaPersona.Id }, nuevaPersona);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _directorioService.EliminarPersona(id);
            return NoContent();
        }
    }
}
