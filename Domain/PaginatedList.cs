using PetStoreApi.Constants;
using PetStoreApi.DTO.PaginationDTO;
using System.Runtime.CompilerServices;

namespace PetStoreApi.Domain
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Content { get; set; }
        public PageInfo PageInfo { get; set; }
        public PaginatedList()
        {
        }

        public PaginatedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            this.Content = items;
            this.PageInfo = new PageInfo();
            this.PageInfo.TotalElements = count;
            this.PageInfo.NumberOfElement = items.Count;
            this.PageInfo.CurrentPage = pageIndex;
            this.PageInfo.TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            this.PageInfo.PageSize = pageSize;
            this.PageInfo.HasNext = pageIndex < pageSize;
            this.PageInfo.HasPrevious = pageIndex > 1;
            this.PageInfo.IsFirst = pageIndex == 1;
            this.PageInfo.IsLast = pageIndex == pageSize;
        }
    }
}
