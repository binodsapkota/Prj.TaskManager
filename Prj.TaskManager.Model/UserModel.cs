using System.ComponentModel.DataAnnotations;

namespace Prj.TaskManager.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }

   
}
