public class OrderDetails {
    public string? orderid { get; set; }
    public string? productid { get; set; }
    public string? unitprice { get; set; }
    public string? quantity { get; set; }
    public string? discount { get; set; }

    public OrderDetails(string[] args) {
        orderid = args[0];
        productid = args[1];
        unitprice = args[2];
        quantity = args[3];
        discount = args[4];
    }
}