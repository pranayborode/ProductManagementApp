using ProductManagementApp.Data;
using ProductManagementApp.Models;
using ProductManagementApp.Models.DTO;
using ProductManagementApp.Repository.Interfaces;

namespace ProductManagementApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        public int AddUser(User user)
        {
            user.Role = "Customer";
            _context.Users.Add(user);
            return _context.SaveChanges();
        }

        public int DeleteUser(int userId)
        {
            int result = 0;
            var user = _context.Users.Find(userId);

            if (user != null)
            {
                _context.Remove(user);
                result = _context.SaveChanges();
                return result;
            }
            return result;
        }

        public int EditUser(User user)
        {
            var _user = _context.Users.Find(user.UserId);

            if(user != null)
            {
                _user.UserName =  user.UserName;
                _user.Password = user.Password;
                _user.Role = user.Role;
                _user.Email = user.Email;
                int result = _context.SaveChanges();
                return result;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public User Login(User user)
        {
            var _user = _context.Users.Where(u => u.UserName == user.UserName && u.Password == user.Password).FirstOrDefault();
            return _user;
        }
    }
}
