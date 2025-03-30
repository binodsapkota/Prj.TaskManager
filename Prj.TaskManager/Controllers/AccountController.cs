using Microsoft.AspNetCore.Mvc;
using Prj.TaskManager.Data;
using Prj.TaskManager.Models;

namespace Prj.TaskManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly AppDbContext _appDbContext;
        public AccountController(AuthService authService, AppDbContext appDbContext)
        {
            _authService = authService;
            _appDbContext = appDbContext;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var result = _authService.AuthenticateUser(HttpContext, model.UserName, model.Password).Result;
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        public IActionResult Logout()
        {

            return RedirectToAction("Login");
        }

        public IActionResult NoPermission()
        {
            return View();
        }
    }
}
