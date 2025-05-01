using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "First name can only contain letters and digits without spaces.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Last name can only contain letters and digits without spaces.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        // Full name is a computed property, not stored in the database.
        public string FullName => $"{FirstName} {LastName}";
    }
}
