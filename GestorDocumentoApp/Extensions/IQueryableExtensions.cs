using GestorDocumentoApp.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace GestorDocumentoApp.Extensions
{
    public static class IQueryableExtensions
    {

        public static async Task<PagedList<T>> ToPagedListAsync<T>(
            this IQueryable<T> source, int pageNumber, int pageSize)
        {
            int page = pageNumber < 1 ? 1 : pageNumber;
            int size = pageSize < 1 ? 1 : pageSize;

            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * size)
                                    .Take(size)
                                    .ToListAsync();

            return new PagedList<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = count
            };
        }
    }
}
