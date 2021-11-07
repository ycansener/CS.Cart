using CS.Shared.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static CS.Shared.Domain.Enums.Enums;

namespace CS.Shared.Domain.Contracts.Services
{
    public interface ICartService
    {
        IEnumerable<ICartItem> GetCart(int userId);
        CartActionResult AddItemToCart(int userId, int productId, int quantity);
        CartActionResult RemoveItemFromCart(int userId, int itemId);
        CartActionResult UpdateCartItemQuantity(int userId, int itemId, int quantity);
    }
}
