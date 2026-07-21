namespace DigitalShop.Application.DTOs.User
{
    public class UserQueryParameters
    {
        private const int MaxPageSize = 100;

        // Pagination
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        // ------------------------------------------------------------
        // Safety control for how a page size is retrieved and modified.
        // get => _pageSize;: Returns the current value stored in the private backing field _pageSize.
        // set => _pageSize = value > MaxPageSize ? MaxPageSize : value;: Evaluates the newly assigned value using a ternary operator:
        // 1) If value is greater than MaxPageSize, it caps the page size at the MaxPageSize.
        // 2) Otherwise, it accepts the assigned value as normal.
        // ------------------------------------------------------------

        // Filter properties
        public string? SearchUser { get; set; }
        public string? SearchUserEmail { get; set; }
    }
}
