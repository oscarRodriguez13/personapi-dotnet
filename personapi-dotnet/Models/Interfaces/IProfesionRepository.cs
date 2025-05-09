using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Interfaces
{
    public interface IProfesionRepository
    {
        Task<IEnumerable<Profesion>> GetAllAsync();
        Task<Profesion> GetByIdAsync(int id);
        Task<Profesion> CreateAsync(Profesion profesion);
        Task<Profesion> UpdateAsync(Profesion profesion);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}