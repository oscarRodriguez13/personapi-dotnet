using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Repository.Interfaces
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> GetAllAsync();
        Task<Persona> GetByIdAsync(int id);
        Task<Persona> CreateAsync(Persona persona);
        Task<Persona> UpdateAsync(Persona persona);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}