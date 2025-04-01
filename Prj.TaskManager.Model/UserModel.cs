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
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public string EmailConfirmationToken { get; set; }
    }

   
}
