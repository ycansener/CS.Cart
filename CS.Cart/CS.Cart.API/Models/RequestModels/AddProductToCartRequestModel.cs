namespace CS.Cart.API.Models.RequestModels
{
    public class AddProductToCartRequestModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
