using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Cart.API.Models.RequestModels
{
    public class RemoveItemFromCartRequestModel
    {
        public int UserId { get; set; }
        public int CartItemId { get; set; }
    }
}
