namespace Basket.API.Entities;
public class ShoppingCart
{
    public string UserName { get; set; } = string.Empty;
    public IEnumerable<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    public ShoppingCart() { }

    public ShoppingCart(string userName)
        => UserName = userName;

    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            Items.ToList()
                .ForEach(i => totalPrice += i.Price * i.Quantity);
            return totalPrice;
        }
    }
}
