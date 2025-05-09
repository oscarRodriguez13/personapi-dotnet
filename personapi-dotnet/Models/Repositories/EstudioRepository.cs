using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Repositories
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
                .Include(e => e.CcPerNavigation)    // O usa .Include(e => e.Persona)
                .Include(e => e.IdProfNavigation)   // O usa .Include(e => e.Profesion)
                .ToListAsync();
        }

        public async Task<Estudio> GetByIdAsync(int idProf, int ccPer)
        {
            return await _context.Estudios
                .Include(e => e.CcPerNavigation)    // O usa .Include(e => e.Persona)
                .Include(e => e.IdProfNavigation)   // O usa .Include(e => e.Profesion)
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
        }

        public async Task<IEnumerable<Estudio>> GetByPersonaIdAsync(int ccPer)
        {
            return await _context.Estudios
                .Include(e => e.IdProfNavigation)   // O usa .Include(e => e.Profesion)
                .Where(e => e.CcPer == ccPer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Estudio>> GetByProfesionIdAsync(int idProf)
        {
            return await _context.Estudios
                .Include(e => e.CcPerNavigation)    // O usa .Include(e => e.Persona)
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
            _context.Entry(estudio).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return estudio;
        }

        public async Task<bool> DeleteAsync(int idProf, int ccPer)
        {
            var estudio = await _context.Estudios
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
            if (estudio == null)
            {
                return false;
            }
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