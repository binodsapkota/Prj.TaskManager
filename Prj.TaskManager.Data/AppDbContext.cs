
using Microsoft.EntityFrameworkCore;
using Prj.TaskManager.Models;
using System.Security.Cryptography;
using System.Text;

namespace Prj.TaskManager.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<TaskItemModel> Tasks { get; set; }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //this is how, we can seed data in database.
            builder.Entity<UserModel>().HasData(
                new UserModel()
                {
                    Id = 1,
                    UserName = "admin",
                    PasswordHash = HashPassword("password"),
                    Role = "admin",
                    Email = "binod@riddhasoft.com",
                    EmailConfirmationToken="",
                    IsEmailConfirmed=true,
                }
                );
        }
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
