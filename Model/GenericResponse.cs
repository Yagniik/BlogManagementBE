namespace BlogMaster.Model
{
    public class GenericResponse<T>
    {
        public string StatusMessage { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
    }
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class SearchParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? Id { get; set; }
        public string Direction { get; set; } = "desc";
        public string SearchTerm { get; set; }
        public string SortBy { get; set; } = "date";
    }
}
