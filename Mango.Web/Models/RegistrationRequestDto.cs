using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class RegistrationRequiredDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
