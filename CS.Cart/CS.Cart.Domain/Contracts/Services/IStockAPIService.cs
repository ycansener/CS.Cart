using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Shared.Domain.Contracts.Services
{
    /// <summary>
    /// Service to manipulate StockAPI endpoints
    /// </summary>
    public interface IStockAPIService
    {
        bool CheckStockForProduct(int productId, int quantity);
    }
}
