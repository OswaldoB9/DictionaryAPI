using DictionaryAPI.DTOs_1;
using Microsoft.EntityFrameworkCore;

namespace DictionaryAPI.Services.Interfaces
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResponseDto<T>> ToPagedAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize)
        {
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponseDto<T>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }
    }
}
