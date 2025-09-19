using DirectorioCore.Interfaces;
using DirectorioCore.Models;

namespace DirectorioAPI.Services
{
    public class VentasService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly ILogger<VentasService> _logger;

        public VentasService(
              IFacturaRepository facturaRepository,
              IPersonaRepository personaRepository,
              ILogger<VentasService> logger)
        {
            _facturaRepository = facturaRepository;
            _personaRepository = personaRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<FacturaViewModel>> ObtenerTodasFacturas()
        {
            _logger.LogInformation("Obteniendo todas las facturas");
            var facturas = await _facturaRepository.GetAllAsync();

            return facturas.Select(f => new FacturaViewModel
            {
                Id = f.Id,
                NumeroFactura = f.NumeroFactura,
                Monto = f.Monto,
                Fecha = f.Fecha,
                PersonaId = f.PersonaId,
                Descripcion = f.Descripcion,
                NombrePersona = $"{f.Persona?.Nombre} {f.Persona?.ApellidoPaterno}",
                IdentificacionPersona = f.Persona?.Identificacion
            });
        }

        public async Task<IEnumerable<FacturaViewModel>> ObtenerFacturasPorPersona(int personaId)
        {
            _logger.LogInformation($"Obteniendo facturas para persona ID: {personaId}");
            var facturas = await _facturaRepository.GetByPersonaIdAsync(personaId);

            return facturas.Select(f => new FacturaViewModel
            {
                Id = f.Id,
                NumeroFactura = f.NumeroFactura,
                Monto = f.Monto,
                Fecha = f.Fecha,
                PersonaId = f.PersonaId,
                Descripcion = f.Descripcion,
                NombrePersona = $"{f.Persona?.Nombre} {f.Persona?.ApellidoPaterno}",
                IdentificacionPersona = f.Persona?.Identificacion
            });
        }

        public async Task<Factura> ObtenerFacturaPorId(int id)
        {
            _logger.LogInformation($"Obteniendo factura con ID: {id}");
            return await _facturaRepository.GetByIdAsync(id);
        }

        public async Task<Factura> AlmacenarFactura(Factura factura)
        {
            _logger.LogInformation($"Almacenando factura: {factura.NumeroFactura}");
            var persona = await _personaRepository.GetByIdAsync(factura.PersonaId);
            if (persona == null)
            {
                throw new KeyNotFoundException($"Persona con ID {factura.PersonaId} no encontrada");
            }

            return await _facturaRepository.AddAsync(factura);
        }

        public async Task EliminarFactura(int id)
        {
            _logger.LogInformation($"Eliminando factura con ID: {id}");
            await _facturaRepository.DeleteAsync(id);
        }
    }
}
