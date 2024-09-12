using ProductManagementApp.Models;
using ProductManagementApp.Models.DTO;

namespace ProductManagementApp.Repository.Interfaces
{
    public interface IUserRepository
    {
        User Login(User user);
        int AddUser(User user);

        int EditUser(User user);

        int DeleteUser(int userId);

        User GetUserById(int userId);

        IEnumerable<User> GetAllUsers();
    }
}
