namespace PetStoreApi.Domain
{
    public class PageParam
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public PageParam()
        {
        }
        public PageParam(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
