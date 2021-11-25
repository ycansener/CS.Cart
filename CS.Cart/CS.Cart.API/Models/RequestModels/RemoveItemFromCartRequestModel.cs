namespace CS.Cart.API.Models.RequestModels
{
    public class RemoveItemFromCartRequestModel
    {
        public int UserId { get; set; }
        public int CartItemId { get; set; }
    }
}
