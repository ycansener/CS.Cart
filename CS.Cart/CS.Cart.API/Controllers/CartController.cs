using CS.Cart.API.Models.RequestModels;
using CS.Shared.Domain.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CS.Cart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetCart")]
        public async Task<IActionResult> GetCartAsync(int userId)
        {
            try
            {
                var result = await _cartService.GetCartAsync(userId);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while fetching the cart.");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddProductToCartAsync([FromBody] AddProductToCartRequestModel requestModel)
        {
            try
            {
                var result = await _cartService.AddItemToCartAsync(requestModel.UserId, requestModel.ProductId, requestModel.Quantity);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while adding an item to the cart.");
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantityAsync([FromBody] UpdateQuantityRequestModel requestModel)
        {
            try
            {
                var result = await _cartService.UpdateCartItemQuantityAsync(requestModel.UserId, requestModel.CartItemId, requestModel.Quantity);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while updating an item to the cart.");
                return StatusCode(500);
            }
        }


        [HttpDelete]
        [Route("RemoveFromCart")]
        public async Task<IActionResult> RemoveItemFromCartAsync([FromBody] RemoveItemFromCartRequestModel requestModel)
        {
            try
            {
                var result = await _cartService.RemoveItemFromCartAsync(requestModel.UserId, requestModel.CartItemId);

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while removing an item from the cart.");
                return StatusCode(500);
            }
        }
    }
}
