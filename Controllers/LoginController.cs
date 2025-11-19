using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Interfaces;
using tl2_tp8_2025_Gonz0x.ViewModels;

namespace tl2_tp8_2025_Gonz0x.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // [HttpGet] Muestra la vista de login
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        // [HttpPost] Procesa el login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Debe ingresar usuario y contraseña.";
                return View("Index", model);
            }

            if (_authenticationService.Login(model.Username, model.Password))
            {
                return RedirectToAction("Index", "Home");
            }

            model.ErrorMessage = "Credenciales inválidas.";
            return View("Index", model);
        }

        // [HttpGet] Cierra sesión
        public IActionResult Logout()
        {
            _authenticationService.Logout();
            return RedirectToAction("Index");
        }
    }
}
