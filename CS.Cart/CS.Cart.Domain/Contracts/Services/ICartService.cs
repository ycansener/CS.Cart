using CS.Shared.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.Shared.Domain.Contracts.Services
{
    public interface ICartService
    {
        Task<IEnumerable<ICartItem>> GetCartAsync(int userId);
        Task<bool> AddItemToCartAsync(int userId, int productId, int quantity);
        Task<bool> RemoveItemFromCartAsync(int userId, int itemId);
        Task<bool> UpdateCartItemQuantityAsync(int userId, int itemId, int quantity);
    }
}
