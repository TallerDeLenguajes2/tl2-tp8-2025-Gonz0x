using tl2_tp8_2025_Gonz0x.Models;

namespace tl2_tp8_2025_Gonz0x.Interfaces
{
    public interface IAuthenticationService
    {
        bool Login(string username, string password);
        void Logout();
        bool IsAuthenticated();
        Usuario? CurrentUser(); // opcional para obtener info del usuario
        bool HasAccessLevel(string requiredRole); // e.g. "Administrador"
    }
}
