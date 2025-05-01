using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        public string ImageUrl { get; set; }

        [Range(0, 1000, ErrorMessage = "Stock quantity cannot be negative or exceed 1000")]
        public int StockQuantity { get; set; }

        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category { get; set; } = "Electronics"; // Default to Electronics
    }
}
