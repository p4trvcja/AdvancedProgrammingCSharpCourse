public class Order {
    public string? orderid { get; set; }
    public string? customerid { get; set; }
    public string? employeeid { get; set; }
    public string? orderdate { get; set; }
    public string? requireddate { get; set; }
    public string? shippeddate { get; set; }
    public string? shipvia { get; set; }
    public string? freight { get; set; }
    public string? shipname { get; set; }
    public string? shipaddress { get; set; }
    public string? shipcity { get; set; }
    public string? shipregion { get; set; }
    public string? shippostalcode { get; set; }
    public string? shipcountry { get; set; }

    public Order(string[] args) {
        orderid = args[0];
        customerid = args[1];
        employeeid = args[2];
        orderdate = args[3];
        requireddate = args[4];
        shippeddate = args[5];
        shipvia = args[6];
        freight = args[7];
        shipname = args[8];
        shipaddress = args[9];
        shipcity = args[10];
        shipregion = args[11];
        shippostalcode = args[12];
        shipcountry = args[13];
    }

}