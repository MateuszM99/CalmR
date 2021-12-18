using System.Linq;
using Application.Common.DTO;

namespace Application.Common.Helpers
{
    public class PagingService<T>
    {
        protected virtual IQueryable<T> ApplyPaging(IQueryable<T> q, PageDTO page)
        {
            if (page == null)
            {
                return q;
            }

            int pageIndex = (page.PageIndex ?? 0);
            int skipIndex = pageIndex > 0 ? (pageIndex - 1) : 0;
            
            return q.Skip(skipIndex * (page.PageSize ?? 10)).Take((page.PageSize ?? 10));
        }
    }
}