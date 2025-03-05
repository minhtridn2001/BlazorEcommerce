using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Data
{
    public class ProductCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "-";
    }
}
