using Microsoft.AspNetCore.Mvc;
using Prj.TaskManager.Data;
using Prj.TaskManager.Models;
using Prj.TaskManager.Service;

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
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel()
                {
                    UserName = model.UserName,
                    Role = model.Role,
                   
                };

                var result = await _authService.Register(user,model.Password);
                return RedirectToAction("login");
            }

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout(HttpContext);
            return RedirectToAction("Login");
        }

        public IActionResult NoPermission()
        {
            return View();
        }
    }
}
