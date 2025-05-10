using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Repository
{
    public class ProfesionRepository : IProfesionRepository
    {
        private readonly PersonaDbContext _context;

        public ProfesionRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profesion>> GetAllAsync()
        {
            return await _context.Profesions
                .Include(p => p.Estudios)
                    .ThenInclude(e => e.Persona)
                .ToListAsync();
        }

        public async Task<Profesion> GetByIdAsync(int id)
        {
            return await _context.Profesions
                .Include(p => p.Estudios)
                    .ThenInclude(e => e.Persona)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Profesion> CreateAsync(Profesion profesion)
        {
            _context.Profesions.Add(profesion);
            await _context.SaveChangesAsync();
            return profesion;
        }

        public async Task<Profesion> UpdateAsync(Profesion profesion)
        {
            _context.Profesions.Update(profesion);
            await _context.SaveChangesAsync();
            return profesion;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var profesion = await _context.Profesions.FindAsync(id);
            if (profesion == null)
            {
                return false;
            }

            _context.Profesions.Remove(profesion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Profesions.AnyAsync(p => p.Id == id);
        }
    }
}