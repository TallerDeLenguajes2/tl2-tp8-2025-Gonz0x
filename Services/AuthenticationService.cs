using tl2_tp8_2025_Gonz0x.Models;
using Microsoft.AspNetCore.Http;
using tl2_tp8_2025_Gonz0x.Interfaces;

namespace tl2_tp8_2025_Gonz0x.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Login(string username, string password)
        {
            var context = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext no está disponible.");

            var user = _userRepository.GetUser(username, password);

            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("User", user.User);
            context.Session.SetString("UserNombre", user.Nombre);
            context.Session.SetString("Rol", user.Rol);
        }




        public void Logout()
        {
            var context = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext no está disponible.");

            context.Session.Clear();
        }

        public bool IsAuthenticated()
        {
            var context = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext no está disponible.");

            return context.Session.GetString("IsAuthenticated") == "true";
        }

        public bool HasAccessLevel(string requiredAccessLevel)
        {
            var context = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext no está disponible.");

            return context.Session.GetString("Rol") == requiredAccessLevel;
        }

        public Usuario? CurrentUser()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return null;

            var username = context.Session.GetString("User");
            if (username == null) return null;

            return new Usuario
            {
                User = username,
                Nombre = context.Session.GetString("UserNombre"),
                Rol = context.Session.GetString("Rol")
            };
        }
    }
}
