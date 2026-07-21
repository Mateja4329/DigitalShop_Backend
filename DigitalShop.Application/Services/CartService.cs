using DigitalShop.Application.DTOs.CartDTO;
using DigitalShop.Application.Mappings;
using DigitalShop.Infrastructure.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Application.Services
{
    public class CartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        
    }
}
