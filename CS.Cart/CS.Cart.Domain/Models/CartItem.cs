namespace CS.Shared.Domain.Models
{
    public class CartItem : ICartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get ; set ; }
        public int Quantity { get ; set ; }
        public decimal ProductPrice { get; set; }

        public decimal GetItemTotal()
        {
            return ProductPrice * Quantity;
        }
    }
}
