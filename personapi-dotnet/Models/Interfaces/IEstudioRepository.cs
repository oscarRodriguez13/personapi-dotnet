using personapi_dotnet.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Interfaces
{
    public interface IEstudioRepository
    {
        Task<IEnumerable<Estudio>> GetAllAsync();
        Task<Estudio> GetByIdAsync(int idProf, int ccPer);
        Task<IEnumerable<Estudio>> GetByPersonaIdAsync(int ccPer);
        Task<IEnumerable<Estudio>> GetByProfesionIdAsync(int idProf);
        Task<Estudio> CreateAsync(Estudio estudio);
        Task<Estudio> UpdateAsync(Estudio estudio);
        Task<bool> DeleteAsync(int idProf, int ccPer);
        Task<bool> ExistsAsync(int idProf, int ccPer);
    }
}