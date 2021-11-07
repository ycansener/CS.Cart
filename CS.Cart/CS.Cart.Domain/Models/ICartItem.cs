using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Shared.Domain.Models
{
    public interface ICartItem
    {
        int Id { get; set; }
        int UserId { get; set; }
        int ProductId { get; set; }
        decimal ProductPrice { get; set; }
        int Quantity { get; set; }

        decimal GetItemTotal();
    }
}
