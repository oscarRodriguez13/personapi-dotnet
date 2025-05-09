using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Models.Interfaces;
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers
{
    public class EstudiosController : Controller
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

        // GET: Estudios
        public async Task<IActionResult> Index()
        {
            var estudios = await _estudioRepository.GetAllAsync();
            return View(estudios);
        }

        // GET: Estudios/Details/5/10
        public async Task<IActionResult> Details(int? idProf, int? ccPer)
        {
            if (idProf == null || ccPer == null)
            {
                return NotFound();
            }

            var estudio = await _estudioRepository.GetByIdAsync(idProf.Value, ccPer.Value);
            if (estudio == null)
            {
                return NotFound();
            }

            return View(estudio);
        }

        // GET: Estudios/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CcPer"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Nombre");
            ViewData["IdProf"] = new SelectList(await _profesionRepository.GetAllAsync(), "Id", "Nom");
            return View();
        }

        // POST: Estudios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            if (ModelState.IsValid)
            {
                if (await _estudioRepository.ExistsAsync(estudio.IdProf, estudio.CcPer))
                {
                    ModelState.AddModelError("", "Ya existe un registro de estudio con esta combinación de profesión y persona.");
                    ViewData["CcPer"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Nombre", estudio.CcPer);
                    ViewData["IdProf"] = new SelectList(await _profesionRepository.GetAllAsync(), "Id", "Nom", estudio.IdProf);
                    return View(estudio);
                }

                await _estudioRepository.CreateAsync(estudio);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CcPer"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Nombre", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(await _profesionRepository.GetAllAsync(), "Id", "Nom", estudio.IdProf);
            return View(estudio);
        }

        // GET: Estudios/Edit/5/10
        public async Task<IActionResult> Edit(int? idProf, int? ccPer)
        {
            if (idProf == null || ccPer == null)
            {
                return NotFound();
            }

            var estudio = await _estudioRepository.GetByIdAsync(idProf.Value, ccPer.Value);
            if (estudio == null)
            {
                return NotFound();
            }
            ViewData["CcPer"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Nombre", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(await _profesionRepository.GetAllAsync(), "Id", "Nom", estudio.IdProf);
            return View(estudio);
        }

        // POST: Estudios/Edit/5/10
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idProf, int ccPer, [Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!await _estudioRepository.ExistsAsync(estudio.IdProf, estudio.CcPer))
                {
                    return NotFound();
                }

                await _estudioRepository.UpdateAsync(estudio);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CcPer"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Nombre", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(await _profesionRepository.GetAllAsync(), "Id", "Nom", estudio.IdProf);
            return View(estudio);
        }

        // GET: Estudios/Delete/5/10
        public async Task<IActionResult> Delete(int? idProf, int? ccPer)
        {
            if (idProf == null || ccPer == null)
            {
                return NotFound();
            }

            var estudio = await _estudioRepository.GetByIdAsync(idProf.Value, ccPer.Value);
            if (estudio == null)
            {
                return NotFound();
            }

            return View(estudio);
        }

        // POST: Estudios/Delete/5/10
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idProf, int ccPer)
        {
            var result = await _estudioRepository.DeleteAsync(idProf, ccPer);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}