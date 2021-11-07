using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Shared.Domain.Contracts.Services
{
    /// <summary>
    /// Service to manipulate ProductAPI endpoints
    /// </summary>
    public interface IProductAPIService
    {
        decimal GetProductPrice(int productId);
    }
}
