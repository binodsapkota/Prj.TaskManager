using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Prj.TaskManager.Data;
using Prj.TaskManager.Models;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Prj.TaskManager.Service
{
    public class AuthService: IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<bool> AuthenticateUser(HttpContext httpContext, string userName, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return false;
            }
            else if (!VerifyPassword(password, user.PasswordHash))
            {
                return false;
            }
            //sign in process


            ///roles define and store
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, user.Role)
               
            };
            ///session store
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //persistent cookie/login

            var authProperties = new AuthenticationProperties() { IsPersistent = true };


            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity),
                authProperties
                );


            return true;
        }


        public async Task<UserModel> Register(UserModel model,string password)
        {
            model.PasswordHash = HashPassword(password);
            _context.Users.Add(model);
            await _context.SaveChangesAsync();//await waits for result and proceed to next line
            //if you dont use await it will not wait for completion
            return model;
        }
        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }
        private bool VerifyPassword(string inputPassword, string passwordHash)
        {
            return HashPassword(inputPassword) == passwordHash;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
