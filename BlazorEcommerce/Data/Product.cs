
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Data
{
    [Index(nameof(CreatedDate))]
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(0, double.MaxValue, MinimumIsExclusive = true, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }

    }
}
