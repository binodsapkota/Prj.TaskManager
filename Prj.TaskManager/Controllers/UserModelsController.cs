using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prj.TaskManager.Data;
using Prj.TaskManager.Filters;
using Prj.TaskManager.Models;
using Prj.TaskManager.Service;

namespace Prj.TaskManager.Controllers
{
    [CustomAuthorize(roles: "admin")]
    public class UserModelsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;
        private readonly EmailService _emailService;

        public UserModelsController(AppDbContext context,AuthService authService, EmailService emailService)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
        }

        // GET: UserModels
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> users = new List<UserViewModel>();

            users = await (from c in _context.Users
                           select new UserViewModel()
                           {
                               Email = c.Email,
                               EmailConfirmationToken = c.EmailConfirmationToken,
                               IsEmailConfirmed = c.IsEmailConfirmed,
                               Id = c.Id,
                               Role = c.Role,
                               UserName = c.UserName,
                           }
             ).ToListAsync();

            return View(users);
        }

        // GET: UserModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: UserModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Role,Email,IsEmailConfirmed,EmailConfirmationToken")] UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel()
                {
                    UserName = userModel.UserName,
                    Role = userModel.Role,
                    Email = userModel.Email
                };

                user.EmailConfirmationToken = Guid.NewGuid().ToString();//random token to validate email

                var result = await _authService.Register(user, userModel.Password);

                var link = Url.Action("ConfirmEmail", "Account", new { token = user.EmailConfirmationToken }, Request.Scheme);

                await _emailService.SendEmail2Async(user.Email, "Confirm Your Email", $"Click <a href='{link}'>here</a> to confirm your email.");

                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: UserModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: UserModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,PasswordHash,Role,Email,IsEmailConfirmed,EmailConfirmationToken")] UserModel userModel)
        {
            if (id != userModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: UserModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: UserModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userModel = await _context.Users.FindAsync(id);
            if (userModel != null)
            {
                _context.Users.Remove(userModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
