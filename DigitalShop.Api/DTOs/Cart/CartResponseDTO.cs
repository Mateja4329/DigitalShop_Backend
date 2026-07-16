namespace DigitalShop.DTOs.CartDTO
{
    public class CartResponseDTO
    {
        public Guid CartId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public decimal Quantity { get; set; }
    }
}
