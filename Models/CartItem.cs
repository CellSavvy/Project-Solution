using ECommerce.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class CartItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    [Required]
    [Range(1, 10)]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, 100000)]
    [DataType(DataType.Currency)]
    public decimal UnitPrice { get; set; }

    [NotMapped]
    public decimal TotalPrice => Quantity * UnitPrice;
}