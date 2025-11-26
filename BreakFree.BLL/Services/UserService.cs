using System.Linq;
using BreakFree.DAL;
using BreakFree.DAL.Entities;
using BreakFree.BLL.Interfaces;


namespace BreakFree.BLL.Services
{
    
    public class UserService : IUserService
    {
        public User? Login(string email, string password)
        {
            using var context = new BreakFreeContext();
            return context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }


        public bool Register(string username, string email, string password)
        {
            using var context = new BreakFreeContext();

            if (context.Users.Any(u => u.Email == email))
                return false;

            var user = new User
            {
                UserName = username,
                Email = email,
                Password = password
            };

            context.Users.Add(user);
            context.SaveChanges();

            return true;
        }
        

        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            using var context = new BreakFreeContext();

            var user = context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
                return false;


            if (user.Password != currentPassword)
                return false;

            user.Password = newPassword;
            context.SaveChanges();

            return true;
        }


        public bool DeleteUser(int userId, string password)
        {
            using var context = new BreakFreeContext();

            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return false;

            if (user.Password != password)
                return false;

            context.Users.Remove(user);

            context.SaveChanges();

            return true;
        }


        public bool UpdateUser(int userId, string newUsername, string newEmail)
        {
            using var context = new BreakFreeContext();

            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return false;


            if (!newUsername.Equals(user.UserName, StringComparison.OrdinalIgnoreCase))
            {
                if (context.Users.Any(u => u.UserName == newUsername && u.UserId != userId))
                    return false; 
            }


            if (!newEmail.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (context.Users.Any(u => u.Email == newEmail && u.UserId != userId))
                    return false; 
            }

            user.UserName = newUsername;
            user.Email = newEmail;

            context.SaveChanges();
            
            return true;
        }


        public User? GetUserById(int id)
        {
            using var context = new BreakFreeContext();
            return context.Users.FirstOrDefault(u => u.UserId == id);
        }
    }
}
