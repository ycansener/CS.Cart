using CS.Shared.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static CS.Shared.Domain.Enums.Enums;

namespace CS.Shared.Domain.Repositories
{
    public interface ICartRepository
    {
        IEnumerable<ICartItem> GetCart(int cartId);

        CartActionResult AddItemToCart(ICartItem item);
        CartActionResult RemoveItemFromCart(int userId, int itemId);
        CartActionResult UpdateCartItemQuantity(int userId, int itemId, int quantity);
        ICartItem GetCartItem(int userId, int cartItemId);
    }
}
