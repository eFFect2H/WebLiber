using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebLib
{
    // <summary>
    /// Параметры сортировки.
    /// </summary>
    public class SortingFilter
    {
        public string? OrderBy { get; set; }
        public bool Descending { get; set; } = false;
    }
}
