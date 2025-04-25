using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DEPI_Graduation_Project.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User is required.")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        // To link Notification to Order
        [Required(ErrorMessage = "Message is required.")]
        [MaxLength(250, ErrorMessage = "Message can't exceed 250 characters.")]
        [MinLength(10, ErrorMessage = "Message can't be less than 10 characters")]
        [RegularExpression("^[a-zA-Z1-9 ]+$", ErrorMessage = "Payment Method name can only contain letters, numbers and spaces.")]
        public string Message { get; set; }
        // The message the notification will hold
        [Required(ErrorMessage = "Creation Time is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime CreationTime { get; set; }
    }

}
