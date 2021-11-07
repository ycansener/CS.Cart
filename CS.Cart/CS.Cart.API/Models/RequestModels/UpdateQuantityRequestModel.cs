using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Cart.API.Models.RequestModels
{
    public class UpdateQuantityRequestModel
    {
        public int UserId { get; set; }
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
