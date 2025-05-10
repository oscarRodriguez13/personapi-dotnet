using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelefonosApiController : ControllerBase
    {
        private readonly ITelefonoRepository _repository;

        public TelefonosApiController(ITelefonoRepository repository)
        {
            _repository = repository;
        }

        // GET: api/TelefonosApi
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Telefono>>> GetTelefonos()
        {
            var telefonos = await _repository.GetAllAsync();
            return Ok(telefonos);
        }

        // GET: api/TelefonosApi/5551234567
        [HttpGet("{numero}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Telefono>> GetTelefono(string numero)
        {
            var telefono = await _repository.GetByNumeroAsync(numero);

            if (telefono == null)
            {
                return NotFound();
            }

            return Ok(telefono);
        }

        // GET: api/TelefonosApi/porPersona/5
        [HttpGet("porPersona/{personaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Telefono>>> GetTelefonosPorPersona(int personaId)
        {
            var telefonos = await _repository.GetByPersonaIdAsync(personaId);
            return Ok(telefonos);
        }

        // POST: api/TelefonosApi
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Telefono>> CreateTelefono(Telefono telefono)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nuevoTelefono = await _repository.CreateAsync(telefono);
                return CreatedAtAction(nameof(GetTelefono), new { numero = nuevoTelefono.Num }, nuevoTelefono);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el teléfono: {ex.Message}");
            }
        }

        // PUT: api/TelefonosApi/5551234567
        [HttpPut("{numero}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTelefono(string numero, Telefono telefono)
        {
            if (numero != telefono.Num)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _repository.ExistsAsync(numero))
            {
                return NotFound();
            }

            try
            {
                await _repository.UpdateAsync(telefono);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el teléfono: {ex.Message}");
            }
        }

        // DELETE: api/TelefonosApi/5551234567
        [HttpDelete("{numero}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTelefono(string numero)
        {
            if (!await _repository.ExistsAsync(numero))
            {
                return NotFound();
            }

            await _repository.DeleteAsync(numero);

            return NoContent();
        }
    }
}