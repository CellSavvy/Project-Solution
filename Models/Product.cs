using System.ComponentModel.DataAnnotations;

namespace DEPI_Graduation_Project.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Product name can only contain letters and spaces.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Description can only contain letters, numbers, and spaces.")]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }

        [Required]
        [Range(5, 100000, ErrorMessage = "Price must be greater than 5.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be a positive number.")]
        public int StockQuantity { get; set; }

    }
}
