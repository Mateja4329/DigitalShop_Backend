using DigitalShop.Application.DTOs.CartDTO;
using DigitalShop.Application.DTOs.ProductDTO;
using DigitalShop.Application.DTOs.User;
using DigitalShop.Infrastructure.Entities;
using System.Runtime.CompilerServices;

namespace DigitalShop.Application.Mappings
{
    public static class Mapper
    {
        // ============================== PRODUCT ==============================
        // INPUT: From CreateDTO in first data base (entity)
        // Here we are mapping the ProductCreateDTO to the Product entity for database insertion
        // Client to server
        public static Product ToProductEntity(this ProductCreateDTO product)
        {
            return new Product
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductCategory = product.ProductCategory,
                ProductCondition = product.ProductCondition,
                DateCreated = DateTime.UtcNow,
                Price = product.Price
            };
        }

        // OUTPUT: From data base (Entity) to ResponseDTO for client
        // Here we are mapping the Product entity to the ProductResponseDTO
        // for sending back to the client
        // Server to client
        public static ProductResponseDTO ToProductResponseDto(this Product product)
        {
            return new ProductResponseDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductCategory = product.ProductCategory,
                ProductCondition = product.ProductCondition,
                Price = product.Price
            };
        }

        // The ToDtoList extension method takes a list of Product objects
        // and maps them to a list of ProductResponseDTO objects using the Select LINQ method.
        public static List<ProductResponseDTO> ToProductResponseDtoList(this List<Product> products)
        {
            return products.Select(p => p.ToProductResponseDto()).ToList();
        }

        // ============================== CART ==============================
        public static CartItem ToCartEntity(this CartCreateDTO cart, Guid userId)
        {
            return new CartItem
            {
                UserId = userId,
                ProductId = cart.ProductId,
                Quantity = cart.Quantity
            };
        }

        public static CartResponseDTO ToCartResponseDto(this CartItem cart)
        {
            return new CartResponseDTO
            {
                // We ALWAYS have to add an ID for response, because of frontend
                CartId = cart.CartItemId,
                ProductName = cart.Product.ProductName,
                FirstName = cart.User.FirstName,
                LastName = cart.User.LastName,
                DateAdded = cart.DateAdded,
                Quantity = cart.Quantity
            };
        }

        public static List<CartResponseDTO> ToCartResponseDtoList(this List<CartItem> carts)
        {
            return carts.Select(c => c.ToCartResponseDto()).ToList();
        }

        // ============================== USER ==============================
        public static User ToUserEntity(this UserCreateDTO user, string hashedPassword)
        {
            return new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = hashedPassword,
                PhoneNumber = user.PhoneNumber
            };
        }

        public static UserResponseDTO ToUserResponseDto(this User user)
        {
            return new UserResponseDTO
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public static List<UserResponseDTO> ToUserResponseDtolist(this List<User> users)
        {
            return users.Select(u => u.ToUserResponseDto()).ToList();
        }
    }
}
