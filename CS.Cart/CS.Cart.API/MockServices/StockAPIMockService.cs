using CS.Shared.Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Cart.API.MockServices
{
    public class StockAPIMockService : IStockAPIService
    {
        public bool CheckStockForProduct(int productId, int quantity)
        {
            if (quantity > 10)
                return false;

            return true;
        }
    }
}
