using System.ComponentModel.DataAnnotations;

namespace DEPI_Graduation_Project.Models
{
    public class User
    {
        public enum UserRole { Client, Admin, Moderator }

        [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(100)]
            [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain letters and spaces.")]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            [MaxLength(100)]
            public string Email { get; set; }

            [Required]
            [MinLength(6)]
            [RegularExpression(@"^(?=.[A-Z])(?=.\d)(?=.[!@#$%^&]).{6,}$",
                ErrorMessage = "Password must be at least 6 characters long, contain an uppercase letter, a number, and a special character.")]
            public string PasswordHash { get; set; }

            [Required]
            public UserRole Role { get; set; }

            public List<Order> Orders { get; set; }
            public List<CartItem> Cart { get; set; }
            public List<WishListItem> WishList { get; set; }
        }
    }
