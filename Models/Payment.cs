using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DEPI_Graduation_Project.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Order is required."), ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        // To link the order with the transaction
        [Required(ErrorMessage = "Payment method is required.")]
        [MaxLength(50, ErrorMessage = "Payment method can't exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Payment Method name can only contain letters and spaces.")]
        public string PaymentMethod { get; set; }
        // To show the choosen payment method
        [Required(ErrorMessage = "Payment date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime PaymentDate { get; set; }
        // To show Time when transaction was made
        [Required(ErrorMessage = "Payment status is required.")]
        public bool IsPaid { get; set; }
        // To know whether the order was paid or not
    }
}
