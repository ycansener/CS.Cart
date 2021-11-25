namespace CS.Cart.API.Models.RequestModels
{
    public class UpdateQuantityRequestModel
    {
        public int UserId { get; set; }
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
