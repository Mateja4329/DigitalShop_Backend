namespace DigitalShop.Infrastructure.Entities.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        PaymentAuthorized,
        Processing,
        ReadyForPickup,
        Shipped,
        Delivered,
        Cancelled,
        Refunded
    }
}
