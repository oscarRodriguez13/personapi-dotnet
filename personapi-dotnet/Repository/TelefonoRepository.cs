using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace personapi_dotnet.Repository
{
    public class TelefonoRepository : ITelefonoRepository
    {
        private readonly PersonaDbContext _context;
        public TelefonoRepository(PersonaDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Telefono>> GetAllAsync()
        {
            return await _context.Telefonos
                .Include(t => t.DuenioNavigation)
                .ToListAsync();
        }
        public async Task<Telefono> GetByNumeroAsync(string numero)
        {
            return await _context.Telefonos
                .Include(t => t.DuenioNavigation)
                .FirstOrDefaultAsync(t => t.Num == numero);
        }
        public async Task<IEnumerable<Telefono>> GetByPersonaIdAsync(int duenio)
        {
            return await _context.Telefonos
                .Where(t => t.Duenio == duenio)
                .ToListAsync();
        }
        public async Task<Telefono> CreateAsync(Telefono telefono)
        {
            try
            {
                // Verificar que no estamos siguiendo la entidad Persona si existe
                if (telefono.DuenioNavigation != null)
                {
                    _context.Entry(telefono.DuenioNavigation).State = EntityState.Detached;
                    telefono.DuenioNavigation = null; // Desconectar para evitar problemas
                }

                // Añadir el teléfono sin la propiedad de navegación
                _context.Telefonos.Add(telefono);
                await _context.SaveChangesAsync();

                // Cargar la propiedad de navegación después de guardar
                await _context.Entry(telefono)
                    .Reference(t => t.DuenioNavigation)
                    .LoadAsync();

                return telefono;
            }
            catch (Exception ex)
            {
                // Loggear el error para diagnóstico
                Console.WriteLine($"Error al crear teléfono: {ex.Message}");
                throw; // Re-lanzar la excepción para manejarla en el controlador
            }
        }

        public async Task<Telefono> UpdateAsync(Telefono telefono)
        {
            try
            {
                // Verificar que no estamos siguiendo la entidad Persona si existe
                if (telefono.DuenioNavigation != null)
                {
                    _context.Entry(telefono.DuenioNavigation).State = EntityState.Detached;
                    telefono.DuenioNavigation = null; // Desconectar para evitar problemas
                }

                // Verificar si el teléfono existe en la base de datos
                var telefonoExistente = await _context.Telefonos.FindAsync(telefono.Num);
                if (telefonoExistente == null)
                {
                    throw new Exception($"No se encontró el teléfono con número {telefono.Num}");
                }

                // Actualizar solo los campos necesarios
                telefonoExistente.Oper = telefono.Oper;
                telefonoExistente.Duenio = telefono.Duenio;

                // Marcar como modificado y guardar
                _context.Entry(telefonoExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Cargar la propiedad de navegación después de actualizar
                await _context.Entry(telefonoExistente)
                    .Reference(t => t.DuenioNavigation)
                    .LoadAsync();

                return telefonoExistente;
            }
            catch (Exception ex)
            {
                // Loggear el error para diagnóstico
                Console.WriteLine($"Error al actualizar teléfono: {ex.Message}");
                throw; // Re-lanzar la excepción para manejarla en el controlador
            }
        }

        public async Task<bool> DeleteAsync(string numero)
        {
            var telefono = await _context.Telefonos.FindAsync(numero);
            if (telefono == null)
            {
                return false;
            }
            _context.Telefonos.Remove(telefono);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExistsAsync(string numero)
        {
            return await _context.Telefonos.AnyAsync(t => t.Num == numero);
        }
    }
}