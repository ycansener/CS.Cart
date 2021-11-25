using CS.Shared.Domain.Contracts.Services;

namespace CS.Cart.API.MockServices
{
    public class StockAPIMockService : IStockAPIService
    {
        /// <summary>
        /// Checks the available stock for the product with the given id and desired quantity
        /// </summary>
        /// <param name="productId">Id of the product</param>
        /// <param name="quantity">Desired quantity that will be queried</param>
        /// <returns>True if the desired quantity of items are in stock, False if they're not.</returns>
        public bool CheckStockForProduct(int productId, int quantity)
        {
            if (quantity > 10)
                return false;

            return true;
        }
    }
}
