using DirectorioAPI.Services;
using DirectorioCore.Models;
using Microsoft.AspNetCore.Mvc;


namespace DirectorioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : Controller
    {
        private readonly VentasService _ventasService;
        private readonly ILogger<VentasController> _logger;

        public VentasController(VentasService ventasService, ILogger<VentasController> logger)
        {
            _ventasService = ventasService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaViewModel>>> Get()
        {
            try
            {
                var facturas = await _ventasService.ObtenerTodasFacturas();
                return Ok(facturas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener facturas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("persona/{personaId}")]
        public async Task<ActionResult<IEnumerable<FacturaViewModel>>> GetByPersona(int personaId)
        {
            try
            {
                var facturas = await _ventasService.ObtenerFacturasPorPersona(personaId);
                return Ok(facturas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener facturas para persona ID: {personaId}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> Get(int id)
        {
            try
            {
                var factura = await _ventasService.ObtenerFacturaPorId(id);
                if (factura == null) return NotFound();
                return Ok(factura);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener factura ID: {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Factura>> Post(FacturaViewModel factura)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var infoFactura= new Factura
                {
                    NumeroFactura = factura.NumeroFactura,
                    Monto = factura.Monto,
                    Fecha = factura.Fecha,
                    PersonaId = factura.PersonaId,
                    Descripcion = factura.Descripcion
                };


                var nuevaFactura = await _ventasService.AlmacenarFactura(infoFactura);
                return CreatedAtAction(nameof(Get), new { id = nuevaFactura.Id }, nuevaFactura);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear factura");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ventasService.EliminarFactura(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar factura ID: {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
