using AutoFixture;
using CS.Cart.API.Services;
using CS.Shared.Domain.Contracts.Services;
using CS.Shared.Domain.Models;
using CS.Shared.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CS.Cart.Tests
{
    public class CartServiceTests
    {
        private readonly IFixture _fixture;
        private Mock<ICartRepository> _cartRepositoryMock;
        private Mock<IStockAPIService> _stockAPIServiceMock;
        private Mock<IProductAPIService> _productAPIServiceMock;
        private readonly Mock<ILogger<CartService>> _logger;

        public CartServiceTests()
        {
            _fixture = new Fixture();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _stockAPIServiceMock = new Mock<IStockAPIService>();
            _productAPIServiceMock = new Mock<IProductAPIService>();
            _logger = new Mock<ILogger<CartService>>();
        }

        private ICartService CreateSut()
        {
            return new CartService(_cartRepositoryMock.Object, _stockAPIServiceMock.Object, _productAPIServiceMock.Object, _logger.Object);
        }

        [Theory]
        [InlineData(-1)]
        public async Task IfParametersNotValid_GetCardShouldReturnEmptyList(int userId)
        {
            var cartService = CreateSut();

            var result = await cartService.GetCartAsync(userId);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(-1, 1, 1)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 1, -1)]
        public async Task IfParametersNotValid_AddItemShouldReturnFalse(int userId, int productId, int quantity)
        {
            _stockAPIServiceMock.Setup(m => m.CheckStockForProduct(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var cartService = CreateSut();

            var result = await cartService.AddItemToCartAsync(userId,productId,quantity);

            Assert.False(result);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        public async Task IfParametersNotValid_RemoveItemShouldReturnFalse(int userId, int itemId)
        {
            var cartService = CreateSut();

            var result = await cartService.RemoveItemFromCartAsync(userId, itemId);

            Assert.False(result);
        }


        [Fact]
        public async Task IfThereIsNotEnoughStock_AddItemShouldReturnFalse()
        {
            int userId = _fixture.Create<int>();
            int productId = _fixture.Create<int>();
            int quantity = _fixture.Create<int>();

            _stockAPIServiceMock.Setup(m => m.CheckStockForProduct(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var cartService = CreateSut();

            var result = await cartService.AddItemToCartAsync(userId, productId, quantity);

            Assert.False(result);
        }


        [Fact]
        public async Task IfThereIsNotEnoughStock_UpdateQuantityShouldReturnFalse()
        {
            int userId = Math.Abs(_fixture.Create<int>());
            int productId = Math.Abs(_fixture.Create<int>());
            int quantity = Math.Abs(_fixture.Create<int>());

            _stockAPIServiceMock.Setup(m => m.CheckStockForProduct(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            _cartRepositoryMock.Setup(m => m.GetCartItemAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new CartItem() {UserId = userId, ProductId = productId, Id = It.IsAny<int>() });
            
            var cartService = CreateSut();

            var result = await cartService.UpdateCartItemQuantityAsync(userId, productId, quantity);

            Assert.False(result);
        }
    }
}
