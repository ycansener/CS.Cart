using CS.Shared.Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Cart.API.MockServices
{
    public class ProductAPIMockService : IProductAPIService
    {
        public decimal GetProductPrice(int productId)
        {
            return productId * 4.5m;
        }
    }
}
