namespace DigitalShop.Application.DTOs.CartDTO
{
    public class CartCreateDTO
    {
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
