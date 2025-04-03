using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prj.TaskManager.Data;
using Prj.TaskManager.Filters;
using Prj.TaskManager.Models;
using Prj.TaskManager.Service;

namespace Prj.TaskManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        
        private readonly EmailService _emailService;
        public AccountController(AuthService authService, EmailService emailService)
        {
            _authService = authService;
            
           _emailService = emailService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _authService.AuthenticateUser(HttpContext, model.UserName, model.Password).Result;
                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [CustomAuthorize]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel()
                {
                    UserName = model.UserName,
                    Role = model.Role,
                    Email=model.Email
                };

                user.EmailConfirmationToken = Guid.NewGuid().ToString();//random token to validate email

                var result = await _authService.Register(user, model.Password);

                var link = Url.Action("ConfirmEmail", "Account", new { token = user.EmailConfirmationToken }, Request.Scheme);

                await _emailService.SendEmail2Async(user.Email, "Confirm Your Email", $"Click <a href='{link}'>here</a> to confirm your email.");

                return RedirectToAction("login");
            }

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout(HttpContext);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            var confirmation = await _authService.ConfirmEmail(token);
            if (confirmation)
            {
                return RedirectToAction("login");
            }
            return NotFound("Invalid Token");
        }

        public IActionResult NoPermission()
        {
            return View();
        }
    }
}
