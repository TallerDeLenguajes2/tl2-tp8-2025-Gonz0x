using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Interfaces;
using tl2_tp8_2025_Gonz0x.ViewModels;

namespace tl2_tp8_2025_Gonz0x.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(
            IAuthenticationService authenticationService,
            ILogger<LoginController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try {
                if (!ModelState.IsValid) return View("Index", model);

                _authenticationService.Login(model.Username, model.Password);

                // ✅ LOG ACCESO EXITOSO (Consigna 1.1)
                _logger.LogInformation("El usuario {Usuario} ingresó correctamente", model.Username);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex) {
                // ⚠️ LOG ACCESO RECHAZADO (Consigna 1.2)
                _logger.LogWarning("Intento de acceso inválido + Usuario: {Usuario} + Clave ingresada: {Clave}", 
                    model.Username, model.Password);

                // ❌ LOG ERROR SERIALIZADO (Consigna 1.3)
                _logger.LogError(ex.ToString());

                model.ErrorMessage = ex.Message;
                return View("Index", model);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _authenticationService.Logout();
            _logger.LogInformation("Usuario cerró sesión");

            return RedirectToAction("Index");
        }
    }
}
