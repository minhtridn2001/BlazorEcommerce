using Microsoft.EntityFrameworkCore;
using BlazorEcommerce.Shared.DTO;
using System.Linq.Expressions;

namespace BlazorEcommerce.Data.Pagination
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Simple pagination implementation.
        /// </summary>
        /// <typeparam name="T">Must be an Entity type to be sortable</typeparam>
        /// <param name="query"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public static async Task<PagedResultDTO<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query, int pageNumber, int pageSize,
        string? orderBy = null, bool ascending = true)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var totalItems = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.PropertyOrField(parameter, orderBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = ascending ? "OrderBy" : "OrderByDescending";
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName
                        && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), property.Type);

                query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
            }
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDTO<T>(items, totalItems, pageNumber, pageSize);
        }
    }
}
