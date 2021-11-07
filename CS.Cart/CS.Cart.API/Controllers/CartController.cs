using CS.Cart.API.Models.RequestModels;
using CS.Shared.Domain.Contracts.Services;
using CS.Shared.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CS.Shared.Domain.Enums.Enums;

namespace CS.Cart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IProductAPIService _productAPIService;

        public CartController(ICartService cartService, IProductAPIService productAPIService)
        {
            _cartService = cartService;
            _productAPIService = productAPIService;
        }

        [HttpGet]
        [Route("GetCart")]
        public IEnumerable<ICartItem> GetCart(int userId)
        {
            return _cartService.GetCart(userId);
        }

        [HttpPost]
        [Route("AddToCart")]
        public CartActionResult AddProductToCart([FromBody] AddProductToCartRequestModel requestModel)
        {
            return _cartService.AddItemToCart(requestModel.UserId, requestModel.ProductId, requestModel.Quantity);
        }

        [HttpPost]
        [Route("UpdateQuantity")]
        public CartActionResult UpdateQuantity([FromBody] UpdateQuantityRequestModel requestModel)
        {
            return _cartService.UpdateCartItemQuantity(requestModel.UserId, requestModel.CartItemId, requestModel.Quantity);
        }


        [HttpPost]
        [Route("RemoveFromCart")]
        public CartActionResult RemoveItemFromCart([FromBody] RemoveItemFromCartRequestModel requestModel)
        {
            return _cartService.RemoveItemFromCart(requestModel.UserId, requestModel.CartItemId);
        }
    }
}
