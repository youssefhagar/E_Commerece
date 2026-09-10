using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Common
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(IReadOnlyList<TEntity> date, int pageIndex, int pageSize, int count)
        {
            Date = date;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
        }

        public IReadOnlyList<TEntity> Date { get; set; } = [];
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }


    }
}
