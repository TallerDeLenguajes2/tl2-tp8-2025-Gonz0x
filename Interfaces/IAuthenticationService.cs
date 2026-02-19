using tl2_tp8_2025_Gonz0x.Models;

namespace tl2_tp8_2025_Gonz0x.Interfaces
{
    public interface IAuthenticationService
    {
        void Login(string username, string password);
        void Logout();
        bool IsAuthenticated();
        Usuario? CurrentUser();
        bool HasAccessLevel(string requiredRole);
    }
}

