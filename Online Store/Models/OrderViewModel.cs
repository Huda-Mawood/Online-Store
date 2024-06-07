using Online_Store.Models;

public class OrderViewModel
{
    public User User { get; set; }
    public Order Order { get; set; }
    public Payment Payment { get; set; }
    public List<ProductCart> ProductCarts { get; set; }
}
