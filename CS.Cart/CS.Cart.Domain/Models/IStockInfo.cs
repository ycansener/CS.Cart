using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Shared.Domain.Models
{
    public interface IStockInfo
    {
        int ProductId { get; set; }
        int Stock { get; set; }
    }
}
