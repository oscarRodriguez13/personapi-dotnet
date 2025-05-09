using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Interfaces
{
    public interface ITelefonoRepository
    {
        Task<IEnumerable<Telefono>> GetAllAsync();
        Task<Telefono> GetByNumeroAsync(string numero);
        Task<IEnumerable<Telefono>> GetByPersonaIdAsync(int duenio);
        Task<Telefono> CreateAsync(Telefono telefono);
        Task<Telefono> UpdateAsync(Telefono telefono);
        Task<bool> DeleteAsync(string numero);
        Task<bool> ExistsAsync(string numero);
    }
}