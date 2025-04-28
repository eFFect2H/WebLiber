using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebLib
{
    public static class CrudServiceExtensions
    {
        /// <summary>
        /// Применение фильтров к IQueryable
        /// </summary>
        public static IQueryable<TEntity> ApplyQuery<TEntity>(this IQueryable<TEntity> query, QueryFilter filter)
        {
            if (filter.Sorting?.OrderBy != null)
            {
                query = filter.Sorting.Descending
                    ? query.OrderByDescending(e => EF.Property<object>(e, filter.Sorting.OrderBy))
                    : query.OrderBy(e => EF.Property<object>(e, filter.Sorting.OrderBy));
            }

            if (filter.Pagination != null)
            {
                int skip = (filter.Pagination.PageNumber - 1) * filter.Pagination.PageSize;
                query = query.Skip(skip).Take(filter.Pagination.PageSize);
            }

            return query;
        }
    }
}
