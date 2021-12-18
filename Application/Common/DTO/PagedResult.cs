using System;
using System.Collections;
using System.Collections.Generic;

namespace Application.Common.DTO
{
    public class PagedResult<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int? PageSize { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PagedResult(int? pageIndex, int? pageSize,int dataCount, IEnumerable<T> data)
        {
            PageIndex = (pageIndex ?? 1);
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(dataCount / (double)(pageSize ?? 10));
            Data = data;
        }
    }
}