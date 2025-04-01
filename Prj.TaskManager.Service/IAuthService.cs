using Microsoft.AspNetCore.Http;
using Prj.TaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj.TaskManager.Service
{
    public interface IAuthService
    {
        Task<bool> AuthenticateUser(HttpContext httpContext, string userName, string password);
        Task<UserModel> Register(UserModel model, string password);
        Task Logout(HttpContext httpContext);
        Task<bool> ConfirmEmail(string token);
    }
}
