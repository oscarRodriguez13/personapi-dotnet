using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotne.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesionesApiController : ControllerBase
    {
        private readonly IProfesionRepository _profesionRepository;

        public ProfesionesApiController(IProfesionRepository profesionRepository)
        {
            _profesionRepository = profesionRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Profesion>>> GetAll()
        {
            try
            {
                var profesiones = await _profesionRepository.GetAllAsync();
                return Ok(profesiones);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las profesiones: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Profesion>> Get(int id)
        {
            try
            {
                var profesion = await _profesionRepository.GetByIdAsync(id);
                return profesion == null ? NotFound() : Ok(profesion);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la profesión: {ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([Bind("Nom,Des")] Profesion profesion)
        {
            try
            {
                // Eliminar cualquier estado de modelo para Estudios si existe
                if (ModelState.ContainsKey("Estudios"))
                {
                    ModelState.Remove("Estudios");
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Crear un nuevo objeto para evitar problemas de seguimiento
                var nuevaProfesion = new Profesion
                {
                    Nom = profesion.Nom,
                    Des = profesion.Des
                    // No establecemos Estudios aquí
                };

                await _profesionRepository.CreateAsync(nuevaProfesion);
                return CreatedAtAction(nameof(Get), new { id = nuevaProfesion.Id }, nuevaProfesion);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la profesión: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [Bind("Id,Nom,Des")] Profesion profesion)
        {
            try
            {
                if (id != profesion.Id)
                    return BadRequest("El ID de la URL no coincide con el del modelo.");

                // Eliminar cualquier estado de modelo para Estudios si existe
                if (ModelState.ContainsKey("Estudios"))
                {
                    ModelState.Remove("Estudios");
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Verificar que la profesión exista
                if (!await _profesionRepository.ExistsAsync(id))
                {
                    return NotFound($"No se encontró la profesión con ID {id}");
                }

                // En lugar de actualizar directamente la entidad completa, obtenemos la existente
                var existingProfesion = await _profesionRepository.GetByIdAsync(id);

                // Actualizamos sólo las propiedades necesarias
                existingProfesion.Nom = profesion.Nom;
                existingProfesion.Des = profesion.Des;

                // Realizar la actualización
                await _profesionRepository.UpdateAsync(existingProfesion);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la profesión: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!await _profesionRepository.ExistsAsync(id))
                {
                    return NotFound($"No se encontró la profesión con ID {id}");
                }

                var result = await _profesionRepository.DeleteAsync(id);
                if (!result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo eliminar la profesión");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la profesión: {ex.Message}");
            }
        }
    }
}