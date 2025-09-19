using DirectorioCore.Models;

namespace DirectorioCore.Interfaces
{
    public interface IFacturaRepository
    {
        Task<IEnumerable<Factura>> GetByPersonaIdAsync(int personaId);
        Task<IEnumerable<Factura>> GetAllAsync();
        Task<Factura> GetByIdAsync(int id);
        Task<Factura> AddAsync(Factura factura);
        Task UpdateAsync(Factura factura);
        Task DeleteAsync(int id);
        Task DeleteByPersonaIdAsync(int personaId);
    }
}
