using DirectorioCore.Models;

namespace DirectorioCore.Interfaces
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> GetAllAsync();
        Task<Persona> GetByIdAsync(int id);
        Task<Persona> AddAsync(Persona persona);
        Task UpdateAsync(Persona persona);
        Task DeleteAsync(int id);
    }
}
