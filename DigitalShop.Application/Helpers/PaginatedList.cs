namespace DigitalShop.Application.Helpers
{
    public class PaginatedList<T> // This <T> means any object
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        // Practical Scenario:
        // If you have a database with 100 products, and you are displaying 10 products per page:
        // items: Contains only the 10 product objects for the current page.
        // count: 100 (the total number of products in the database).
        // pageIndex: 1 (the current page).
        // pageSize: 10 (items per page).
    }
}
