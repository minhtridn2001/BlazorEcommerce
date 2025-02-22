namespace BlazorEcommerce.Data.Pagination
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; }
        public List<T> Items { get; set; } = new();

        public PagedResult(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Items = items.ToList();
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
        }
    }
}
