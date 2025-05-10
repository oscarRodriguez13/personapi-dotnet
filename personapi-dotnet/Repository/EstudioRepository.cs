using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Repository
{
    public class EstudioRepository : IEstudioRepository
    {
        private readonly PersonaDbContext _context;

        public EstudioRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estudio>> GetAllAsync()
        {
            return await _context.Estudios
                .Include(e => e.CcPerNavigation)
                .Include(e => e.IdProfNavigation)
                .ToListAsync();
        }

        public async Task<Estudio> GetByIdAsync(int idProf, int ccPer)
        {
            return await _context.Estudios
                .Include(e => e.CcPerNavigation)
                .Include(e => e.IdProfNavigation)
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
        }

        public async Task<IEnumerable<Estudio>> GetByPersonaIdAsync(int ccPer)
        {
            return await _context.Estudios
                .Include(e => e.IdProfNavigation)
                .Where(e => e.CcPer == ccPer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Estudio>> GetByProfesionIdAsync(int idProf)
        {
            return await _context.Estudios
                .Include(e => e.CcPerNavigation)
                .Where(e => e.IdProf == idProf)
                .ToListAsync();
        }

        public async Task<Estudio> CreateAsync(Estudio estudio)
        {
            _context.Estudios.Add(estudio);
            await _context.SaveChangesAsync();
            return estudio;
        }

        public async Task<Estudio> UpdateAsync(Estudio estudio)
        {
            var existing = await _context.Estudios
                .FirstOrDefaultAsync(e => e.IdProf == estudio.IdProf && e.CcPer == estudio.CcPer);

            if (existing == null)
                return null;

            existing.Fecha = estudio.Fecha;
            existing.Univer = estudio.Univer;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int idProf, int ccPer)
        {
            var estudio = await _context.Estudios
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);

            if (estudio == null)
                return false;

            _context.Estudios.Remove(estudio);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int idProf, int ccPer)
        {
            return await _context.Estudios.AnyAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
        }
    }
}
