namespace DigitalShop.Application.DTOs.CartDTO
{
    public class CartCreateDTO
    {
        // What, who and how much
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public decimal Quantity { get; set; }
    }
}
