using BreakFree.DAL.Entities;

namespace BreakFree.BLL.Interfaces
{
    public interface IUserService
    {
        User? Login(string email, string password);

        bool Register(string username, string email, string password);

        bool ChangePassword(int userId, string currentPassword, string newPassword);

        bool DeleteUser(int userId, string password);

        bool UpdateUser(int userId, string newUsername, string newEmail);
    }
}
