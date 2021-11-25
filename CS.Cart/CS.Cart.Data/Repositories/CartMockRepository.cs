using CS.Cart.Data.DataModels;
using CS.Shared.Domain.Models;
using CS.Shared.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Cart.Data.Repositories
{
    // Mock DB repository executes CRUD operations on mock data stored in memory.
    public class CartMockRepository : ICartRepository
    {
        // Mock db table
        private static List<CartItemEntityMock> _cartItems;
        private readonly ILogger<CartMockRepository> _logger;

        public CartMockRepository(ILogger<CartMockRepository> logger)
        {
            _logger = logger;

            // Initializing the table
            if (_cartItems == null)
            {
                _cartItems = new List<CartItemEntityMock>();
            }
        }

        /// <summary>
        /// Adds a new item to the cart
        /// </summary>
        /// <param name="item">Item model that contains all necessary information to add the item to the cart</param>
        /// <returns>True if the operation is successfull, False if something went wrong</returns>
        public async Task<bool> AddItemToCartAsync(ICartItem item)
        {
            var cartItems = await GetCartInternalAsync(item.UserId);

            if (cartItems == null)
            {
                _logger.LogWarning($"No items in the cart for user {item.UserId}");
                return false;
            }

            var cartItem = new CartItemEntityMock()
            {
                UserId = item.UserId,
                ProductId = item.ProductId,
                Id = await GetNextValueForCartItemIdAsync(),
                ProductPrice = item.ProductPrice,
                Quantity = item.Quantity
            };

            _cartItems.Add(cartItem);

            return true;
        }

        /// <summary>
        /// Gets the items in the user's cart by userId
        /// </summary>
        /// <param name="userId">Id of the user that owns the cart</param>
        /// <returns>Collection of the items in the cart of the user</returns>
        public async Task<IEnumerable<ICartItem>> GetCartAsync(int userId)
        {
            var cartItems = await GetCartInternalAsync(userId);

            if(cartItems == null)
            {
                return null;
            }

            // This part can be done by a Mapper such as AutoMapper
            return cartItems.Select(p => new CartItem()
            {
                Id = p.Id,
                UserId = p.UserId,
                ProductId = p.ProductId,
                ProductPrice = p.ProductPrice,
                Quantity = p.Quantity,
            });
        }

        /// <summary>
        /// Gets an item from the cart of the user
        /// </summary>
        /// <param name="userId">Id of the user that owns the cart</param>
        /// <param name="cartItemId">Id of the item that will be fetched from the cart of the user</param>
        /// <returns>Item model that represents the item</returns>
        public async Task<ICartItem> GetCartItemAsync(int userId, int cartItemId)
        {
            var cartItem = await GetCartItemInternalAsync(userId, cartItemId);

            if(cartItem == null)
            {
                return null;
            }

            return new CartItem()
            {
                Id = cartItem.Id,
                UserId = cartItem.UserId,
                ProductId = cartItem.ProductId,
                ProductPrice = cartItem.ProductPrice,
                Quantity = cartItem.Quantity,
            };
        }

        /// <summary>
        /// Removes an item from the cart of the user by the user Id and id of the item that will be removed
        /// </summary>
        /// <param name="userId">Id of the user that owns the cart</param>
        /// <param name="itemId">Id of the item that will be removed from the cart of the user</param>
        /// <returns>True if the operation is successfull, False if something went wrong</returns>
        public async Task<bool> RemoveItemFromCartAsync(int userId, int itemId)
        {
            var cartItem = await GetCartItemInternalAsync(userId, itemId);

            if (cartItem == null)
            {
                _logger.LogWarning($"No such item with id:{itemId} found in the cart for user:{userId}");
                return false;
            }

            bool success = _cartItems.Remove(cartItem);
            return success;
        }

        /// <summary>
        /// Updates the quantity of an item in the cart of the user by id of the user, id of the item and the quantity value that will be updated
        /// </summary>
        /// <param name="userId">Id of the user that owns the cart</param>
        /// <param name="itemId">Id of the item that will be removed from the cart of the user</param>
        /// <param name="quantity">Value that will be updated as the quantity of the item</param>
        /// <returns>True if the operation is successfull, False if something went wrong</returns>
        public async Task<bool> UpdateCartItemQuantityAsync(int userId, int itemId, int quantity)
        {
            var cartItem = await GetCartItemInternalAsync(userId, itemId);

            if (cartItem == null)
            {
                _logger.LogWarning($"No such item with id:{itemId} found in the cart for user:{userId}");
                return false;
            }

            cartItem.Quantity = quantity;
            return true;
        }

        /// <summary>
        /// Gets the cart item from the local collection
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="cartItemId">Id of the item to be fetched from the cart </param>
        /// <returns>CartItemEntityMock object that represents the item</returns>
        private async Task<CartItemEntityMock> GetCartItemInternalAsync(int userId, int cartItemId)
        {
            var cartItem = _cartItems.FirstOrDefault(p => p.Id == cartItemId && p.UserId == userId);

            return cartItem;
        }

        /// <summary>
        /// Gets the collection of the items in the cart from the local collection
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Collection of CartItemEntityMock that represents the cart</returns>
        private async Task<IEnumerable<CartItemEntityMock>> GetCartInternalAsync(int userId)
        {
            var cartItems = _cartItems.Where(p => p.UserId == userId);
            return cartItems;
        }

        /// <summary>
        /// Gets the next incremental id value of the local collection
        /// </summary>
        /// <returns>Next id value of the local collection</returns>
        private async Task<int> GetNextValueForCartItemIdAsync()
        {
            var itemWithBiggestId = _cartItems.OrderByDescending(p => p.Id).FirstOrDefault();
            return itemWithBiggestId != null ? itemWithBiggestId.Id + 1 : 1;
        }
    }
}
