using CS.Shared.Domain.Contracts.Services;
using CS.Shared.Domain.Models;
using CS.Shared.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.Cart.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IStockAPIService _stockAPIService;
        private readonly IProductAPIService _productAPIService;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepository cartRepository, IStockAPIService stockAPIService, IProductAPIService productAPIService, ILogger<CartService> logger)
        {
            _logger = logger;
            _cartRepository = cartRepository;
            _stockAPIService = stockAPIService;
            _productAPIService = productAPIService;
        }

        /// <summary>
        /// Adds a new item to the user's cart
        /// </summary>
        /// <param name="userId">Id of the user who owns the cart</param>
        /// <param name="productId">Id of the product that will be added to the cart</param>
        /// <param name="quantity">The desired quantity of the product that will be added to the cart</param>
        /// <returns>True if the operation is successfull, False if something went wrong</returns>
        public async Task<bool> AddItemToCartAsync(int userId, int productId, int quantity)
        {
            if (userId < 1 || productId < 1 || quantity < 1)
            {
                _logger.LogWarning($"Invalid parameters: UserId:{userId} ProductId:{productId} Quantity:{quantity}");
                return false;
            }

            bool stockAvailable = _stockAPIService.CheckStockForProduct(productId, quantity);

            if (!stockAvailable)
            {
                _logger.LogWarning($"Out of stock: UserId:{userId} ProductId:{productId} Quantity:{quantity}");
                return false;
            }

            decimal upToDatePrice = _productAPIService.GetProductPrice(productId);

            return await _cartRepository.AddItemToCartAsync(new CartItem()
            {
                ProductId = productId,
                ProductPrice = upToDatePrice,
                Quantity = quantity,
                UserId = userId
            });
        }

        /// <summary>
        /// Gets the items in the user's cart by userId
        /// </summary>
        /// <param name="userId">Id of the user that owns the cart</param>
        /// <returns>Collection of items in the cart</returns>
        public async Task<IEnumerable<ICartItem>> GetCartAsync(int userId)
        {
            if (userId < 1)
            {
                return null;
            }

            return await _cartRepository.GetCartAsync(userId);
        }

        /// <summary>
        /// Removes an item from the user's cart by the given user Id and item Id
        /// </summary>
        /// <param name="userId">Id of the user that owns the cart</param>
        /// <param name="itemId">Id of the item that will be removed from the cart</param>
        /// <returns>True if the operation is successfull, False if something went wrong</returns>
        public async Task<bool> RemoveItemFromCartAsync(int userId, int itemId)
        {
            if (userId < 1 || itemId < 1)
            {
                _logger.LogWarning($"Invalid parameters: UserId:{userId} ItemId:{itemId}");
                return false;
            }

            return await _cartRepository.RemoveItemFromCartAsync(userId, itemId);
        }

        /// <summary>
        /// Updates the quantity of an item in the user's cart by the userId and the itemId
        /// </summary>
        /// <param name="userId">Id of the user that owns the cart</param>
        /// <param name="itemId">Id of the item that will be updated</param>
        /// <param name="quantity">The value that item quantity will be updated with</param>
        /// <returns>True if the operation is successfull, False if something went wrong.</returns>
        public async Task<bool> UpdateCartItemQuantityAsync(int userId, int itemId, int quantity)
        {
            if (userId < 1 || itemId < 1 || quantity < 1)
            {
                _logger.LogWarning($"Invalid parameters: UserId:{userId} ItemId:{itemId} Quantity:{quantity}");
                return false;
            }

            var cartItem = await _cartRepository.GetCartItemAsync(userId, itemId);

            if (cartItem == null)
            {
                _logger.LogWarning($"Item not found: UserId:{userId} ItemId:{itemId} Quantity:{quantity}");
                return false;
            }

            bool stockAvailable = _stockAPIService.CheckStockForProduct(cartItem.ProductId, quantity);

            if (!stockAvailable)
            {
                _logger.LogWarning($"Out of stock: UserId:{userId} ProductId:{cartItem.ProductId} Quantity:{quantity}");
                return false;
            }

            return await _cartRepository.UpdateCartItemQuantityAsync(userId, itemId, quantity);
        }
    }
}
