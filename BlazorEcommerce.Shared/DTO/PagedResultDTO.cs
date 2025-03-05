using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared.DTO
{
    public class PagedResultDTO<T>
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new();

        public PagedResultDTO() { } // Add a default constructor for deserialization

        public PagedResultDTO(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Items = items.ToList();
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
        }
    }
}
