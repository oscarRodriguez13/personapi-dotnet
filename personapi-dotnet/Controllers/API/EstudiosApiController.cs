using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace personapi_dotnet.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiosApiController : ControllerBase
    {
        private readonly IEstudioRepository _repository;

        public EstudiosApiController(IEstudioRepository repository)
        {
            _repository = repository;
        }

        // GET: api/EstudiosApi
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Estudio>>> GetEstudios()
        {
            var estudios = await _repository.GetAllAsync();
            return Ok(estudios);
        }

        // GET: api/EstudiosApi/profesion/5/persona/10
        [HttpGet("profesion/{idProf}/persona/{ccPer}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Estudio>> GetEstudio(int idProf, int ccPer)
        {
            var estudio = await _repository.GetByIdAsync(idProf, ccPer);

            if (estudio == null)
            {
                return NotFound();
            }

            return Ok(estudio);
        }

        // GET: api/EstudiosApi/porPersona/5
        [HttpGet("porPersona/{ccPer}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Estudio>>> GetEstudiosPorPersona(int ccPer)
        {
            var estudios = await _repository.GetByPersonaIdAsync(ccPer);
            return Ok(estudios);
        }

        // GET: api/EstudiosApi/porProfesion/5
        [HttpGet("porProfesion/{idProf}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Estudio>>> GetEstudiosPorProfesion(int idProf)
        {
            var estudios = await _repository.GetByProfesionIdAsync(idProf);
            return Ok(estudios);
        }

        // POST: api/EstudiosApi
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Estudio>> CreateEstudio(Estudio estudio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.CreateAsync(estudio);

            return CreatedAtAction(
                nameof(GetEstudio),
                new { idProf = estudio.IdProf, ccPer = estudio.CcPer },
                estudio);
        }

        // PUT: api/EstudiosApi/profesion/5/persona/10
        [HttpPut("profesion/{idProf}/persona/{ccPer}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEstudio(int idProf, int ccPer, Estudio estudio)
        {
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _repository.ExistsAsync(idProf, ccPer))
            {
                return NotFound();
            }

            var updated = await _repository.UpdateAsync(estudio);

            if (updated == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/EstudiosApi/profesion/5/persona/10
        [HttpDelete("profesion/{idProf}/persona/{ccPer}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEstudio(int idProf, int ccPer)
        {
            if (!await _repository.ExistsAsync(idProf, ccPer))
            {
                return NotFound();
            }

            await _repository.DeleteAsync(idProf, ccPer);

            return NoContent();
        }
    }
}