using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace BlazorEcommerce.Data
{
    [PrimaryKey(nameof(UserId), nameof(ProductId))]
    public class CartItem
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
