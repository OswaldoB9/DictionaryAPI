namespace DictionaryAPI.DTOs_1
{
    public class PaginationQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public void Normalize()
        {
            if (Page < 1) Page = 1;
            if (PageSize < 1) PageSize = 20;
            if (PageSize > 100) PageSize = 100;
        }
    }
}
