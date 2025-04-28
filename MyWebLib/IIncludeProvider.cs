using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyWebLib
{
    /// <summary>
    /// Интерфейс для получения выражений Include.
    /// </summary>
    public interface IIncludeProvider<TEntity>
    {
        IEnumerable<Expression<Func<TEntity, object>>> Includes();
    }
}
