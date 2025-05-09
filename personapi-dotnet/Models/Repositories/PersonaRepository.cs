using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Models.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly PersonaDbContext _context;

        public PersonaRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Estudios)
                    .ThenInclude(e => e.Profesion)
                .ToListAsync();
        }

        public async Task<Persona> GetByIdAsync(int id)
        {
            return await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Estudios)
                    .ThenInclude(e => e.Profesion)
                .FirstOrDefaultAsync(p => p.Cc == id);
        }

        public async Task<Persona> CreateAsync(Persona persona)
        {
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
            return persona;
        }

        public async Task<Persona> UpdateAsync(Persona persona)
        {
            _context.Entry(persona).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return persona;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return false;
            }

            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Personas.AnyAsync(p => p.Cc == id);
        }
    }
}