using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Params
{
    public class ProductQueryParams
    {
        public int ? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? SearchValue { get; set; } =default!;

        public ProductSortingOption Sort { get; set; }

        public int PageIndex { get; set; } = 1;
        private const int maxPageSize = 50;
        private const int defaultPageSize = 10;
        private int pageSize = defaultPageSize;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > maxPageSize) ? maxPageSize : (value < 1 ? defaultPageSize : value);
        }


    }
}
