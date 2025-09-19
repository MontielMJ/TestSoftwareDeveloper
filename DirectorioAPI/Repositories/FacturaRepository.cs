using DirectorioAPI.Data;
using DirectorioCore.Interfaces;
using DirectorioCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectorioAPI.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly DirectorioDbContext _context;
        private readonly ILogger<FacturaRepository> _logger;

        public FacturaRepository(DirectorioDbContext context, ILogger<FacturaRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Factura>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todas las facturas");
            return await _context.Facturas
                .Include(f => f.Persona)
                .ToListAsync();
        }

        public async Task<Factura> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Obteniendo factura con ID: {id}");
            return await _context.Facturas
                .Include(f => f.Persona)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Factura>> GetByPersonaIdAsync(int personaId)
        {
            _logger.LogInformation($"Obteniendo facturas para persona ID: {personaId}");
            return await _context.Facturas
                .Include(f => f.Persona)
                .Where(f => f.PersonaId == personaId)
                .ToListAsync();
        }

        public async Task<Factura> AddAsync(Factura factura)
        {
            _logger.LogInformation($"Agregando nueva factura: {factura.NumeroFactura}");
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();
            return factura;
        }

        public async Task UpdateAsync(Factura factura)
        {
            _logger.LogInformation($"Actualizando factura con ID: {factura.Id}");
            _context.Facturas.Update(factura);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"Eliminando factura con ID: {id}");
            var factura = await _context.Facturas.FindAsync(id);
            if (factura != null)
            {
                _context.Facturas.Remove(factura);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByPersonaIdAsync(int personaId)
        {
            _logger.LogInformation($"Eliminando facturas para persona ID: {personaId}");
            var facturas = await _context.Facturas
                .Where(f => f.PersonaId == personaId)
                .ToListAsync();

            if (facturas.Any())
            {
                _context.Facturas.RemoveRange(facturas);
                await _context.SaveChangesAsync();
            }
        }
    }
}
