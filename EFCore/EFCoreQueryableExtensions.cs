using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public static class EFCoreQueryableExtensions
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        public static async Task<List<T>> ToPageListAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize,
            RefAsync<int>? totalNumber = null,
            RefAsync<int>? totalPage = null,
            CancellationToken cancellationToken = default)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 1;
            }

            if (totalNumber != null || totalPage != null)
            {
                var total = await query.CountAsync(cancellationToken);
                if (totalNumber != null)
                {
                    totalNumber.Value = total;
                }

                if (totalPage != null)
                {
                    totalPage.Value = (int)Math.Ceiling(total / (double)pageSize);
                }
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
