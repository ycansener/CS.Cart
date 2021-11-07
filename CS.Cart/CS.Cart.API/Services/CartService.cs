using CS.Shared.Domain.Contracts.Services;
using CS.Shared.Domain.Enums;
using CS.Shared.Domain.Models;
using CS.Shared.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Cart.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IStockAPIService _stockAPIService;
        private readonly IProductAPIService _productAPIService;

        public CartService(ICartRepository cartRepository, IStockAPIService stockAPIService, IProductAPIService productAPIService)
        {
            _cartRepository = cartRepository;
            _stockAPIService = stockAPIService;
            _productAPIService = productAPIService;
        }

        public Enums.CartActionResult AddItemToCart(int userId, int productId, int quantity)
        {
            if (userId < 1 || productId < 1 || quantity < 1)
            {
                return Enums.CartActionResult.InvalidOperation;
            }

            bool stockAvailable = _stockAPIService.CheckStockForProduct(productId, quantity);

            if (!stockAvailable)
                return Enums.CartActionResult.OutOfStock;

            decimal upToDatePrice = _productAPIService.GetProductPrice(productId);

            return _cartRepository.AddItemToCart(new CartItem()
            {
                ProductId = productId,
                ProductPrice = upToDatePrice,
                Quantity = quantity,
                UserId = userId
            });
        }

        public IEnumerable<ICartItem> GetCart(int userId)
        {
            if (userId < 1)
            {
                return null;
            }

            return _cartRepository.GetCart(userId);
        }

        public Enums.CartActionResult RemoveItemFromCart(int userId, int itemId)
        {
            if (userId < 1 || itemId < 1)
            {
                return Enums.CartActionResult.InvalidOperation;
            }

            return _cartRepository.RemoveItemFromCart(userId, itemId);
        }

        public Enums.CartActionResult UpdateCartItemQuantity(int userId, int itemId, int quantity)
        {
            if (userId < 1 || itemId < 1 || quantity < 1)
            {
                return Enums.CartActionResult.InvalidOperation;
            }

            var cartItem = _cartRepository.GetCartItem(userId, itemId);

            if(cartItem == null)
            {
                return Enums.CartActionResult.InvalidOperation;
            }

            bool stockAvailable = _stockAPIService.CheckStockForProduct(cartItem.ProductId, quantity);

            if (!stockAvailable)
                return Enums.CartActionResult.OutOfStock;

            return _cartRepository.UpdateCartItemQuantity(userId, itemId, quantity);
        }
    }
}
