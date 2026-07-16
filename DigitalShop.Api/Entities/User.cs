using System.ComponentModel.DataAnnotations;

namespace DigitalShop.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
