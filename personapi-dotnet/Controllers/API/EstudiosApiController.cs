using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiosController : ControllerBase
    {
        private readonly IEstudioRepository _estudioRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IProfesionRepository _profesionRepository;

        public EstudiosController(
            IEstudioRepository estudioRepository,
            IPersonaRepository personaRepository,
            IProfesionRepository profesionRepository)
        {
            _estudioRepository = estudioRepository;
            _personaRepository = personaRepository;
            _profesionRepository = profesionRepository;
        }

        // GET: api/Estudios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudio>>> GetEstudios()
        {
            var estudios = await _estudioRepository.GetAllAsync();
            return Ok(estudios);
        }

        // GET: api/Estudios/5/10
        [HttpGet("{idProf}/{ccPer}")]
        public async Task<ActionResult<Estudio>> GetEstudio(int idProf, int ccPer)
        {
            var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);

            if (estudio == null)
            {
                return NotFound();
            }

            return Ok(estudio);
        }

        // POST: api/Estudios
        [HttpPost]
        public async Task<ActionResult<Estudio>> PostEstudio(Estudio estudio)
        {
            // Verificar que existan la persona y la profesión
            var persona = await _personaRepository.GetByIdAsync(estudio.CcPer);
            var profesion = await _profesionRepository.GetByIdAsync(estudio.IdProf);

            if (persona == null || profesion == null)
            {
                return BadRequest("La persona o profesión especificada no existe");
            }

            // Cargar propiedades de navegación manualmente
            estudio.CcPerNavigation = persona;
            estudio.IdProfNavigation = profesion;

            try
            {
                await _estudioRepository.CreateAsync(estudio);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno al crear el estudio: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetEstudio), new { idProf = estudio.IdProf, ccPer = estudio.CcPer }, estudio);
        }

        // PUT: api/Estudios/5/10
        [HttpPut("{idProf}/{ccPer}")]
        public async Task<IActionResult> PutEstudio(int idProf, int ccPer, Estudio estudio)
        {
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
            {
                return BadRequest("Los identificadores en la URL no coinciden con los datos del estudio");
            }

            // Verificar que existan la persona y la profesión
            var persona = await _personaRepository.GetByIdAsync(estudio.CcPer);
            var profesion = await _profesionRepository.GetByIdAsync(estudio.IdProf);

            if (persona == null || profesion == null)
            {
                return BadRequest("La persona o profesión especificada no existe");
            }

            // Verificar que el estudio a actualizar existe
            var existingEstudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
            if (existingEstudio == null)
            {
                return NotFound($"No se encontró el estudio con IdProf={idProf} y CcPer={ccPer}");
            }

            // Cargar propiedades de navegación
            estudio.CcPerNavigation = persona;
            estudio.IdProfNavigation = profesion;

            try
            {
                await _estudioRepository.UpdateAsync(estudio);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno al actualizar el estudio: {ex.Message}");
            }

            return NoContent();
        }

        // DELETE: api/Estudios/5/10
        [HttpDelete("{idProf}/{ccPer}")]
        public async Task<IActionResult> DeleteEstudio(int idProf, int ccPer)
        {
            var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
            if (estudio == null)
            {
                return NotFound($"No se encontró el estudio con IdProf={idProf} y CcPer={ccPer}");
            }

            var result = await _estudioRepository.DeleteAsync(idProf, ccPer);
            if (!result)
            {
                return StatusCode(500, "Ocurrió un error al eliminar el estudio");
            }

            return NoContent();
        }
    }
}