using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebLib
{
    public class QueryFilter
    {
        public PaginationFilter? Pagination { get; set; }
        public SortingFilter? Sorting { get; set; }
        public Dictionary<string, string>? Filters { get; set; }
    }
}
