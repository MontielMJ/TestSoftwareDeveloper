using DirectorioAPI.Data;
using DirectorioCore.Interfaces;
using DirectorioCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectorioAPI.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly DirectorioDbContext _context;
        private readonly ILogger<PersonaRepository> _logger;

        public PersonaRepository(DirectorioDbContext context, ILogger<PersonaRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todas las personas");
            return await _context.Personas.ToListAsync();
        }

        public async Task<Persona> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Obteniendo persona con ID: {id}");
            return await _context.Personas.FindAsync(id);
        }

        public async Task<Persona> AddAsync(Persona persona)
        {
            _logger.LogInformation($"Agregando nueva persona: {persona.Nombre}");
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
            return persona;
        }

        public async Task UpdateAsync(Persona persona)
        {
            _logger.LogInformation($"Actualizando persona con ID: {persona.Id}");
            _context.Personas.Update(persona);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"Eliminando persona con ID: {id}");
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
                await _context.SaveChangesAsync();
            }
        }
    }
}
