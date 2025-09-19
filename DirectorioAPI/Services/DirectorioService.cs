using DirectorioCore.Interfaces;
using DirectorioCore.Models;

namespace DirectorioAPI.Services
{
    public class DirectorioService
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IFacturaRepository _facturaRepository;
        private readonly ILogger<DirectorioService> _logger;

        public DirectorioService(IPersonaRepository personaRepository,
                               IFacturaRepository facturaRepository,
                               ILogger<DirectorioService> logger)
        {
            _personaRepository = personaRepository;
            _facturaRepository = facturaRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Persona>> ObtenerTodasPersonas()
        {
            return await _personaRepository.GetAllAsync();
        }

        public async Task<Persona> ObtenerPersonaPorId(int id)
        {
            return await _personaRepository.GetByIdAsync(id);
        }

        public async Task<Persona> AlmacenarPersona(Persona persona)
        {
            return await _personaRepository.AddAsync(persona);
        }

        public async Task EliminarPersona(int id)
        {
            await _facturaRepository.DeleteByPersonaIdAsync(id);
            await _personaRepository.DeleteAsync(id);
            _logger.LogInformation($"Persona con ID {id} y sus facturas eliminadas");
        }
    }
}
