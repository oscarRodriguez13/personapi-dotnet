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
        private readonly ILogger<EstudiosController> _logger;

        public EstudiosController(
            IEstudioRepository estudioRepository,
            IPersonaRepository personaRepository,
            IProfesionRepository profesionRepository,
            ILogger<EstudiosController> logger)
        {
            _estudioRepository = estudioRepository;
            _personaRepository = personaRepository;
            _profesionRepository = profesionRepository;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            var estudios = await _estudioRepository.GetAllAsync();
            return View(estudios);
        }

        public async Task<IActionResult> Details(int idProf, int ccPer)
        {
            var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
            if (estudio == null)
            {
                return NotFound();
            }
            return View(estudio);
        }

        public async Task<IActionResult> Create()
        {
            await LoadViewDataAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProf,CcPer,Fecha,Univer")] Estudio estudio)
        {
            _logger.LogInformation("POST Create llamado con datos: IdProf={IdProf}, CcPer={CcPer}, Fecha={Fecha}, Univer={Univer}",
                estudio.IdProf, estudio.CcPer, estudio.Fecha, estudio.Univer);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState válido");

                if (await _estudioRepository.ExistsAsync(estudio.IdProf, estudio.CcPer))
                {
                    _logger.LogWarning("Estudio ya existe con IdProf={IdProf}, CcPer={CcPer}", estudio.IdProf, estudio.CcPer);
                    ModelState.AddModelError("", "Ya existe un estudio con esta combinación de profesión y persona.");
                    await LoadViewDataAsync(estudio);
                    return View(estudio);
                }

                await _estudioRepository.CreateAsync(estudio);
                _logger.LogInformation("Estudio creado correctamente. Redirigiendo a Index.");
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("ModelState inválido");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning("Error de modelo: {ErrorMessage}", error.ErrorMessage);
            }

            await LoadViewDataAsync(estudio);
            return View(estudio);
        }


        public async Task<IActionResult> Edit(int idProf, int ccPer)
        {
            var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
            if (estudio == null)
            {
                return NotFound();
            }

            await LoadViewDataAsync(estudio);
            return View(estudio);
        }

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
                if (!await _estudioRepository.ExistsAsync(idProf, ccPer))
                {
                    return NotFound();
                }

                await _estudioRepository.UpdateAsync(estudio);
                return RedirectToAction(nameof(Index));
            }

            await LoadViewDataAsync(estudio);
            return View(estudio);
        }

        public async Task<IActionResult> Delete(int idProf, int ccPer)
        {
            var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
            if (estudio == null)
            {
                return NotFound();
            }

            return View(estudio);
        }

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

        private async Task LoadViewDataAsync(Estudio estudio = null)
        {
            var personas = await _personaRepository.GetAllAsync();
            var profesiones = await _profesionRepository.GetAllAsync();

            ViewData["CcPer"] = new SelectList(personas, "Cc", "Nombre", estudio?.CcPer);
            ViewData["IdProf"] = new SelectList(profesiones, "Id", "Nom", estudio?.IdProf);
        }
    }
}
