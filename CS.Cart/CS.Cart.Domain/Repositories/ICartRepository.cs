using CS.Shared.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.Shared.Domain.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<ICartItem>> GetCartAsync(int cartId);

        Task<bool> AddItemToCartAsync(ICartItem item);
        Task<bool> RemoveItemFromCartAsync(int userId, int itemId);
        Task<bool> UpdateCartItemQuantityAsync(int userId, int itemId, int quantity);
        Task<ICartItem> GetCartItemAsync(int userId, int cartItemId);
    }
}
