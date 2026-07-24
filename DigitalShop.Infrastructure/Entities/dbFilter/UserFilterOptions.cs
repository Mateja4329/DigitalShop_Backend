namespace DigitalShop.Infrastructure.Entities.dbFilter
{
    public class UserFilterOptions
    {
        private const int MaxPageSize = 100;

        public int PageIndex { get; set; }
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public string? SearchUser { get; set; }
        public string? SearchUserEmail { get; set; }
    }
}
