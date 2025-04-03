using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Prj.TaskManager.Models
{
    public class UserViewModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        [ValidateNever] 
        public string Password { get; set; }
       
        public string Role { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }
        [ValidateNever]
        public string? EmailConfirmationToken { get; set; }
    }
}
