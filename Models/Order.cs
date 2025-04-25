using System.ComponentModel.DataAnnotations;

namespace DEPI_Graduation_Project.Models
{
    public class Order
    {
        public enum OrderStatus { Pending, Confirmed, Cancelled, Shipped, Delivered }

        [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "User ID is required.")]
            public int UserId { get; set; }
            public User User { get; set; }

            [Required(ErrorMessage = "Order date is required.")]
            public DateTime OrderDate { get; set; }

            [Required(ErrorMessage = "Order status is required.")]
            public OrderStatus Status { get; set; }

            [Required(ErrorMessage = "Shipping address is required."), MaxLength(250, ErrorMessage = "Shipping address is too big."), RegularExpression(@"^[a-zA-Z0-9\s,.-]+$", ErrorMessage = "Shipping address contains invalid characters.")]
            public string ShippingAddress { get; set; }

            public List<OrderItem> OrderItems { get; set; }
            public Payment Payment { get; set; }
        }

    }
