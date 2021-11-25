using CS.Shared.Domain.Contracts.Services;
using System.Threading.Tasks;

namespace CS.Cart.API.MockServices
{
    public class ProductAPIMockService : IProductAPIService
    {
        /// <summary>
        /// Returns the price of the product by it's id
        /// </summary>
        /// <param name="productId">Id of the product</param>
        /// <returns>Price of the product</returns>
        public decimal GetProductPrice(int productId)
        {
            return productId * 4.5m;
        }
    }
}
