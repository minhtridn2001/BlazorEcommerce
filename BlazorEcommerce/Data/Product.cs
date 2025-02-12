using Microsoft.Build.Framework;

namespace BlazorEcommerce.Data
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }

    }
}
