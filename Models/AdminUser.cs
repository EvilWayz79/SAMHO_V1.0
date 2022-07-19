using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SAMHO.Models
{
    public class AdminUser : IdentityUser
    {
        [Key]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
