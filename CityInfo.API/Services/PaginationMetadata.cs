namespace CityInfo.API.Services
{
    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int TotalItemCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPageCount { get; set; }

        public PaginationMetadata(int currentPage, int totalItemCount, int pageSize)
        {
            CurrentPage = currentPage;
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
        }
    }
}