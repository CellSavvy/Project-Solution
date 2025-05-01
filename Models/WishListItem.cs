using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class WishListItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
