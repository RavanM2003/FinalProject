namespace Final_Project.Helper
{
    public class Pagination<T>
    {
        public List<T> Items { get; set; }
        public int PageCount { get; set; }

        public int CurrentPage { get; set; }
        public Pagination(List<T> items, int pageCount, int currentPage)
        {
            this.Items = items;
            this.PageCount = pageCount;
            this.CurrentPage = currentPage;
        }
    }
}
