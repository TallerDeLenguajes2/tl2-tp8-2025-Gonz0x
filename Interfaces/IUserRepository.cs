using tl2_tp8_2025_Gonz0x.Models;

namespace tl2_tp8_2025_Gonz0x.Interfaces
{
    public interface IUserRepository
    {
        // Retorna usuario si credenciales válidas, sino null
        Usuario? GetUser(string username, string password);
        Usuario? GetById(int id);
    }
}
