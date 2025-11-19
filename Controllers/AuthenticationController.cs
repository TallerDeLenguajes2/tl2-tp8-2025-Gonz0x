using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_Gonz0x.Interfaces;
using tl2_tp8_2025_Gonz0x.ViewModels;

namespace tl2_tp8_2025_Gonz0x.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
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

            bool success = _authService.Login(model.Username, model.Password);

            if (success)
                return RedirectToAction("Index", "Home");

            model.ErrorMessage = "Usuario o contraseña incorrectos";
            return View("~/Views/Login/Index.cshtml", model);
        }

        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
