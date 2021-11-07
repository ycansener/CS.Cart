using CS.Cart.Data.DataModels;
using CS.Shared.Domain.Enums;
using CS.Shared.Domain.Models;
using CS.Shared.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS.Cart.Data.Repositories
{
    // Mock DB repository executes CRUD operations on mock data stored in memory.
    public class CartMockRepository : ICartRepository
    {
        // Mock db table
        private static List<CartItemEntityMock> _cartItems;

        public CartMockRepository()
        {
            // Initializing the table
            if (_cartItems == null)
            {
                _cartItems = new List<CartItemEntityMock>();
            }
        }

        public Enums.CartActionResult AddItemToCart(ICartItem item)
        {
            var cartItems = GetCartInternal(item.UserId);

            if(cartItems == null)
            {
                return Enums.CartActionResult.InvalidOperation;
            }

            var cartItem = new CartItemEntityMock()
            {
                UserId = item.UserId,
                ProductId = item.ProductId,
                Id = GetNextValueForCartItemId(),
                ProductPrice = item.ProductPrice,
                Quantity = item.Quantity
            };

            _cartItems.Add(cartItem);

            return Enums.CartActionResult.Success;
        }

        public IEnumerable<ICartItem> GetCart(int userId)
        {
            var cartItems = GetCartInternal(userId);

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

        public ICartItem GetCartItem(int userId, int cartItemId)
        {
            var cartItem = GetCartItemInternal(userId, cartItemId);

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

        public Enums.CartActionResult RemoveItemFromCart(int userId, int itemId)
        {
            var cartItem = GetCartItemInternal(userId, itemId);

            if (cartItem == null)
            {
                return Enums.CartActionResult.InvalidOperation;
            }

            bool success = _cartItems.Remove(cartItem);

            return success ? Enums.CartActionResult.Success : Enums.CartActionResult.InvalidOperation;
        }

        public Enums.CartActionResult UpdateCartItemQuantity(int userId, int itemId, int quantity)
        {
            var cartItem = GetCartItemInternal(userId, itemId);

            if (cartItem == null)
            {
                return Enums.CartActionResult.InvalidOperation;
            }

            cartItem.Quantity = quantity;
            return Enums.CartActionResult.Success;
        }

        private CartItemEntityMock GetCartItemInternal(int userId, int cartItemId)
        {
            var cartItem = _cartItems.FirstOrDefault(p => p.Id == cartItemId && p.UserId == userId);

            return cartItem;
        }

        private IEnumerable<CartItemEntityMock> GetCartInternal(int userId)
        {
            var cartItems = _cartItems.Where(p => p.UserId == userId);
            return cartItems;
        }

        private int GetNextValueForCartItemId()
        {
            var itemWithBiggestId = _cartItems.OrderByDescending(p => p.Id).FirstOrDefault();
            return itemWithBiggestId != null ? itemWithBiggestId.Id + 1 : 1;
        }
    }
}
