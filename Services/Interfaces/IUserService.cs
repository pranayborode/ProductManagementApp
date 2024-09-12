using ProductManagementApp.Models;
using ProductManagementApp.Models.DTO;

namespace ProductManagementApp.Services.Interfaces
{
    public interface IUserService
    {
        LoginDTO Login(User user);
        int AddUser(User user);

        int EditUser(User user);

        int DeleteUser(int userId);

        User GetUserById(int userId);
        IEnumerable<User> GetAllUsers();
    }
}
