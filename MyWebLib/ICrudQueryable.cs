using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebLib
{
    /// <summary>
    /// Интерфейс для запроса с фильтрацией и пагинацией.
    /// </summary>
    public interface ICrudQueryable<TDto>
    {
        Task<IEnumerable<TDto>> QueryAsync(QueryFilter filter);
    }
}
