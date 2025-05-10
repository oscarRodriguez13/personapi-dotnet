using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace personapi_dotnet.Controllers.MVC
{
    public class TelefonoesController : Controller
    {
        private readonly ITelefonoRepository _telefonoRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly ILogger<TelefonoesController> _logger;

        public TelefonoesController(
            ITelefonoRepository telefonoRepository,
            IPersonaRepository personaRepository,
            ILogger<TelefonoesController> logger)
        {
            _telefonoRepository = telefonoRepository;
            _personaRepository = personaRepository;
            _logger = logger;
        }

        // GET: Telefonos
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de teléfonos");
                var telefonos = await _telefonoRepository.GetAllAsync();
                return View(telefonos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de teléfonos");
                TempData["ErrorMessage"] = "Error al cargar la lista de teléfonos: " + ex.Message;
                return View(new List<Telefono>());
            }
        }

        // GET: Telefonos/Details/123456789
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                _logger.LogWarning("ID nulo en Details");
                return NotFound();
            }

            try
            {
                _logger.LogInformation($"Obteniendo detalles del teléfono con ID: {id}");
                var telefono = await _telefonoRepository.GetByNumeroAsync(id);
                if (telefono == null)
                {
                    _logger.LogWarning($"Teléfono con ID {id} no encontrado");
                    return NotFound();
                }

                return View(telefono);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles del teléfono con ID: {id}");
                TempData["ErrorMessage"] = "Error al cargar los detalles del teléfono: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Telefonos/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                _logger.LogInformation("Cargando formulario de creación de teléfono");
                var personas = await _personaRepository.GetAllAsync();
                if (personas == null || !personas.Any())
                {
                    _logger.LogWarning("No hay personas disponibles para asignar como dueños");
                    TempData["WarningMessage"] = "No hay personas disponibles para asignar como dueños de teléfonos. Por favor, cree una persona primero.";
                    return RedirectToAction(nameof(Index));
                }

                ViewData["Duenio"] = new SelectList(personas, "Cc", "Nombre");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el formulario de creación de teléfono");
                TempData["ErrorMessage"] = "Error al cargar el formulario: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Telefonos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            try
            {
                _logger.LogInformation($"Intentando crear teléfono: {telefono.Num}, Operador: {telefono.Oper}, Dueño ID: {telefono.Duenio}");

                // Eliminar cualquier estado de modelo para DuenioNavigation si existe
                if (ModelState.ContainsKey("DuenioNavigation"))
                {
                    ModelState.Remove("DuenioNavigation");
                }

                // Verificaciones manuales básicas
                if (string.IsNullOrEmpty(telefono.Num))
                {
                    ModelState.AddModelError("Num", "El número de teléfono es obligatorio");
                }

                if (string.IsNullOrEmpty(telefono.Oper))
                {
                    ModelState.AddModelError("Oper", "El operador es obligatorio");
                }

                // Verificar que Duenio sea válido
                var persona = await _personaRepository.GetByIdAsync(telefono.Duenio);
                if (persona == null)
                {
                    ModelState.AddModelError("Duenio", "El dueño seleccionado no existe");
                }

                if (ModelState.IsValid)
                {
                    if (await _telefonoRepository.ExistsAsync(telefono.Num))
                    {
                        ModelState.AddModelError("Num", "Ya existe un teléfono con este número");
                        ViewData["Duenio"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Nombre", telefono.Duenio);
                        return View(telefono);
                    }

                    // Crear un nuevo objeto Telefono para evitar problemas de seguimiento
                    var nuevoTelefono = new Telefono
                    {
                        Num = telefono.Num,
                        Oper = telefono.Oper,
                        Duenio = telefono.Duenio
                        // No establecemos DuenioNavigation aquí
                    };

                    await _telefonoRepository.CreateAsync(nuevoTelefono);
                    _logger.LogInformation($"Teléfono creado exitosamente: {nuevoTelefono.Num}");
                    TempData["SuccessMessage"] = "Teléfono creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogWarning("ModelState inválido al crear teléfono");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        _logger.LogWarning($"Error de validación: {error.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear teléfono");
                ModelState.AddModelError("", "Ha ocurrido un error al crear el teléfono: " + ex.Message);
            }

            // Si hay errores, recargamos el dropdown
            try
            {
                ViewData["Duenio"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Nombre", telefono.Duenio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recargar la lista de personas");
                ViewData["Duenio"] = new SelectList(new List<Persona>(), "Cc", "Nombre");
            }

            return View(telefono);
        }


        // Resto del controlador...
        // GET: Telefonos/Edit/123456789
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _telefonoRepository.GetByNumeroAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }

            ViewData["Duenio"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Nombre", telefono.Duenio);
            return View(telefono);
        }

        // POST: Telefonos/Edit/123456789
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (id != telefono.Num)
            {
                _logger.LogWarning($"ID no coincidente en Edit: {id} vs {telefono.Num}");
                return NotFound();
            }

            try
            {
                _logger.LogInformation($"Intentando editar teléfono: {telefono.Num}, Operador: {telefono.Oper}, Dueño ID: {telefono.Duenio}");

                // Eliminar cualquier estado de modelo para DuenioNavigation si existe
                if (ModelState.ContainsKey("DuenioNavigation"))
                {
                    ModelState.Remove("DuenioNavigation");
                }

                // Verificar que el teléfono tenga un número
                if (string.IsNullOrEmpty(telefono.Num))
                {
                    _logger.LogWarning("Intento de editar teléfono sin número");
                    ModelState.AddModelError("Num", "El número de teléfono es obligatorio");
                }

                // Verificar que el operador tenga un valor
                if (string.IsNullOrEmpty(telefono.Oper))
                {
                    _logger.LogWarning("Intento de editar teléfono sin operador");
                    ModelState.AddModelError("Oper", "El operador es obligatorio");
                }

                // Verificar que Duenio sea válido
                var persona = await _personaRepository.GetByIdAsync(telefono.Duenio);
                if (persona == null)
                {
                    _logger.LogWarning($"Dueño con ID {telefono.Duenio} no encontrado");
                    ModelState.AddModelError("Duenio", "El dueño seleccionado no existe");
                }

                if (ModelState.IsValid)
                {
                    if (!await _telefonoRepository.ExistsAsync(telefono.Num))
                    {
                        _logger.LogWarning($"Teléfono con número {telefono.Num} no encontrado para editar");
                        return NotFound();
                    }

                    // Crear un nuevo objeto Telefono para evitar problemas de seguimiento
                    var telefonoActualizado = new Telefono
                    {
                        Num = telefono.Num,
                        Oper = telefono.Oper,
                        Duenio = telefono.Duenio
                        // No establecemos DuenioNavigation aquí
                    };

                    await _telefonoRepository.UpdateAsync(telefonoActualizado);
                    _logger.LogInformation($"Teléfono actualizado exitosamente: {telefonoActualizado.Num}");
                    TempData["SuccessMessage"] = "Teléfono actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogWarning("ModelState inválido al editar teléfono");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        _logger.LogWarning($"Error de validación: {error.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al editar teléfono con ID: {id}");
                ModelState.AddModelError("", "Ha ocurrido un error al editar el teléfono: " + ex.Message);
            }

            // Si hay errores, recargamos el dropdown
            try
            {
                ViewData["Duenio"] = new SelectList(await _personaRepository.GetAllAsync(), "Cc", "Nombre", telefono.Duenio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recargar la lista de personas");
                ViewData["Duenio"] = new SelectList(new List<Persona>(), "Cc", "Nombre");
            }

            return View(telefono);
        }

        // GET: Telefonos/Delete/123456789
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _telefonoRepository.GetByNumeroAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // POST: Telefonos/Delete/123456789
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _telefonoRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}