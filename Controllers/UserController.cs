using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApp.Models;
using ProductManagementApp.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagementApp.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private const int PageSize = 10;
        public UserController(IUserService _userService)
        {
            this._userService = _userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: UserController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var loginUser = _userService.Login(user);

            if (loginUser != null)
            {
               
                var claims = new List<Claim>
          {
              new Claim(ClaimTypes.Name, loginUser.UserName),
              new Claim(ClaimTypes.Email, loginUser.Email),
               new Claim(ClaimTypes.Role, loginUser.Role)

          };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

               
                var principal = new ClaimsPrincipal(claimsIdentity);

               
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                var userClaims = User.Claims.Select(c => new { c.Type, c.Value });
                foreach (var claim in userClaims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }

                TempData["SuccessMessage"] = "Login successful.";
                return RedirectToAction("Index", "Home");
            }
            TempData["ErrorMessage"] = "Invalid login attempt.";

            return View();
        }

        // Logout method
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "Logged out successfully.";
            return RedirectToAction("Login", "User");
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Index(int page = 1)
        {

            var users = _userService.GetAllUsers();


            var paginatedUsers = users
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var totalUsers = users.Count();


            var model = new UserListViewModel
            {
                Users = paginatedUsers,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize)
            };


            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_UserListPartial", model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateUserPartial", new User());
        }

        [HttpPost]
        public IActionResult Create(User user)
        {

            int result = _userService.AddUser(user);
            if (result >= 1)
            {
                TempData["SuccessMessage"] = "User created successfully.";
                return Json(new { success = true });
            }
            else
            {
                TempData["ErrorMessage"] = "Error creating user.";
                return Json(new { success = false, message = "Error creating user" });
            }


        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _userService.GetUserById(id);
            return PartialView("_EditUserPartial", user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            int result = _userService.EditUser(user);
            if (result >= 1)
            {
                TempData["SuccessMessage"] = "User Updated successfully.";
                return Json(new { success = true });
            }
            else
            {
                TempData["ErrorMessage"] = "Error updating user.";
                return Json(new { success = false, message = "Error updating user" });
            }

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _userService.GetUserById(id);
            return PartialView("_DeleteUserPartial", user);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _userService.DeleteUser(id);
            TempData["SuccessMessage"] = "User deleted successfully.";
            return Json(new { success = true });
        }




    }
}
