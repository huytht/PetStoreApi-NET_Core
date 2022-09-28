namespace PetStoreApi.DTO.PaginationDTO
{
    public class PageInfo
    {
        public int TotalElements { get; set; }
        public int NumberOfElement { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }

        public PageInfo()
        {
        }
        public PageInfo(int TotalElements, int NumberOfElement, int CurrentPage, int PageSize, int TotalPage, bool HasNext, bool HasPrevious, bool IsFirst, bool IsLast)
        {
            this.TotalElements = TotalElements;
            this.NumberOfElement = NumberOfElement;
            this.CurrentPage = CurrentPage;
            this.PageSize = PageSize;
            this.TotalPage = TotalPage;
            this.HasNext = HasNext;
            this.HasPrevious = HasPrevious;
            this.IsFirst = IsFirst;
            this.IsLast = IsLast;
        }
    }
}
