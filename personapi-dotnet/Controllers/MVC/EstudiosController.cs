using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repository.Interfaces;
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers.MVC
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
        public async Task<IActionResult> Create(Estudio estudio)
        {
            Console.WriteLine("Intentando crear estudio:");
            Console.WriteLine($"IdProf: {estudio.IdProf}, CcPer: {estudio.CcPer}, Fecha: {estudio.Fecha}, Univer: {estudio.Univer}");

            // Quitar validación de propiedades de navegación que no vienen del formulario
            ModelState.Remove(nameof(estudio.CcPerNavigation));
            ModelState.Remove(nameof(estudio.IdProfNavigation));

            if (ModelState.IsValid)
            {
                // Cargar propiedades de navegación manualmente
                estudio.CcPerNavigation = await _personaRepository.GetByIdAsync(estudio.CcPer);
                estudio.IdProfNavigation = await _profesionRepository.GetByIdAsync(estudio.IdProf);

                Console.WriteLine("Persona encontrada: " + (estudio.CcPerNavigation != null ? "Sí" : "No"));
                Console.WriteLine("Profesión encontrada: " + (estudio.IdProfNavigation != null ? "Sí" : "No"));

                await _estudioRepository.CreateAsync(estudio);
                Console.WriteLine("Estudio creado correctamente.");
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("ModelState inválido:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($" - {error.ErrorMessage}");
            }

            // Recargar SelectList en caso de error
            ViewData["CcPer"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Cc", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(await _profesionRepository.GetAllAsync(), "Id", "Nom", estudio.IdProf);
            return View(estudio);
        }


        public async Task<IActionResult> Edit(int idProf, int ccPer)
        {
            var estudio = await _estudioRepository.GetByIdAsync(idProf, ccPer);
            if (estudio == null) return NotFound();

            ViewData["CcPer"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Cc", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(await _profesionRepository.GetAllAsync(), "Id", "Nom", estudio.IdProf);
            return View(estudio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idProf, int ccPer, Estudio estudio)
        {
            Console.WriteLine("Intentando editar estudio:");
            Console.WriteLine($"IdProf: {estudio.IdProf}, CcPer: {estudio.CcPer}, Fecha: {estudio.Fecha}, Univer: {estudio.Univer}");

            // Quitar validación de propiedades de navegación que no vienen del formulario
            ModelState.Remove(nameof(estudio.CcPerNavigation));
            ModelState.Remove(nameof(estudio.IdProfNavigation));

            if (idProf != estudio.IdProf || ccPer != estudio.CcPer)
            {
                Console.WriteLine("IDs en URL no coinciden con el modelo.");
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                estudio.CcPerNavigation = await _personaRepository.GetByIdAsync(estudio.CcPer);
                estudio.IdProfNavigation = await _profesionRepository.GetByIdAsync(estudio.IdProf);

                Console.WriteLine("ModelState válido. Actualizando estudio...");
                await _estudioRepository.UpdateAsync(estudio);
                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("ModelState inválido:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($" - {error.ErrorMessage}");
            }

            // Recargar SelectList en caso de error
            ViewData["CcPer"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Cc", estudio.CcPer);
            ViewData["IdProf"] = new SelectList(await _profesionRepository.GetAllAsync(), "Id", "Nom", estudio.IdProf);
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
