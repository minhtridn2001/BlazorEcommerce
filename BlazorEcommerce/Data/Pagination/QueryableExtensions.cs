using Microsoft.EntityFrameworkCore;
using BlazorEcommerce.Shared.DTO;

namespace BlazorEcommerce.Data.Pagination
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Simple pagination implementation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PagedResultDTO<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var totalItems = await query.CountAsync();
            // TODO Add Sorting support. Without sorting, the order of items is not guaranteed.
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDTO<T>(items, totalItems, pageNumber, pageSize);
        }
    }
}
