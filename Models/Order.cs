using ECommerce.Models;

public class Order
{
    public int Id { get; set; }
    public int AddressId { get; set; }
    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal Amount { get; set; }

    public virtual Address Address { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}