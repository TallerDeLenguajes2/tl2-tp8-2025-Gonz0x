using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Interfaces;
using tl2_tp8_2025_Gonz0x.ViewModels;

namespace tl2_tp8_2025_Gonz0x.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            IAuthenticationService authService,
            ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Login/Index.cshtml", new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Login/Index.cshtml", model);

            try
            {
                _authService.Login(model.Username, model.Password); // void

                _logger.LogInformation("El usuario {Usuario} ingresó correctamente", model.Username);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Acceso inválido
                _logger.LogWarning(
                    "Intento de acceso inválido. Usuario: {Usuario}. Clave ingresada: {Clave}",
                    model.Username,
                    model.Password
                );

                // Errores en ejecución
                _logger.LogError(ex.ToString());

                model.ErrorMessage = ex.Message;
                return View("~/Views/Login/Index.cshtml", model);
            }
        }



        public IActionResult Logout()
        {
            _authService.Logout();
            _logger.LogInformation("Usuario cerró sesión");

            return RedirectToAction("Index", "Home");
        }
    }
}
