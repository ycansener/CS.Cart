using AutoFixture;
using CS.Cart.API.MockServices;
using CS.Cart.API.Services;
using CS.Cart.Data.Repositories;
using CS.Shared.Domain.Contracts.Services;
using CS.Shared.Domain.Models;
using CS.Shared.Domain.Repositories;
using Moq;
using System;
using Xunit;
using static CS.Shared.Domain.Enums.Enums;

namespace CS.Cart.Tests
{
    public class CartServiceTests
    {
        private readonly IFixture _fixture;
        private Mock<ICartRepository> _cartRepositoryMock;
        private Mock<IStockAPIService> _stockAPIServiceMock;
        private Mock<IProductAPIService> _productAPIServiceMock;

        public CartServiceTests()
        {
            _fixture = new Fixture();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _stockAPIServiceMock = new Mock<IStockAPIService>();
            _productAPIServiceMock = new Mock<IProductAPIService>();
        }

        private ICartService CreateSut()
        {
            return new CartService(_cartRepositoryMock.Object, _stockAPIServiceMock.Object, _productAPIServiceMock.Object);
        }

        [Theory]
        [InlineData(-1)]
        public void IfParametersNotValid_GetCardShouldReturnEmptyList(int userId)
        {
            var cartService = CreateSut();

            var result = cartService.GetCart(userId);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(-1, 1, 1)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 1, -1)]
        public void IfParametersNotValid_AddItemShouldReturnInvalidOperation(int userId, int productId, int quantity)
        {
            _stockAPIServiceMock.Setup(m => m.CheckStockForProduct(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var cartService = CreateSut();

            var result = cartService.AddItemToCart(userId,productId,quantity);

            Assert.Equal(CartActionResult.InvalidOperation, result);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        public void IfParametersNotValid_RemoveItemShouldReturnInvalidOperation(int userId, int itemId)
        {
            var cartService = CreateSut();

            var result = cartService.RemoveItemFromCart(userId, itemId);

            Assert.Equal(CartActionResult.InvalidOperation, result);
        }


        [Fact]
        public void IfThereIsNotEnoughStock_AddItemShouldReturnOutOfStock()
        {
            int userId = _fixture.Create<int>();
            int productId = _fixture.Create<int>();
            int quantity = _fixture.Create<int>();

            _stockAPIServiceMock.Setup(m => m.CheckStockForProduct(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var cartService = CreateSut();

            var result = cartService.AddItemToCart(userId, productId, quantity);

            Assert.Equal(CartActionResult.OutOfStock, result);
        }


        [Fact]
        public void IfThereIsNotEnoughStock_UpdateQuantityShouldReturnOutOfStock()
        {
            int userId = Math.Abs(_fixture.Create<int>());
            int productId = Math.Abs(_fixture.Create<int>());
            int quantity = Math.Abs(_fixture.Create<int>());

            _stockAPIServiceMock.Setup(m => m.CheckStockForProduct(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            _cartRepositoryMock.Setup(m => m.GetCartItem(It.IsAny<int>(), It.IsAny<int>())).Returns(new CartItem() {UserId = userId, ProductId = productId, Id = It.IsAny<int>() });
            
            var cartService = CreateSut();

            var result = cartService.UpdateCartItemQuantity(userId, productId, quantity);

            Assert.Equal(CartActionResult.OutOfStock, result);
        }
    }
}
