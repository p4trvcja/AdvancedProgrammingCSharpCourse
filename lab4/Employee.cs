public class Employee {
    public string? employeeid { get; set; }
    public string? lastname { get; set; }
    public string? firstname { get; set; }
    public string? title { get; set; }
    public string? titleofcourtesy { get; set; }
    public string? birthdate { get; set; }
    public string? hiredate { get; set; }
    public string? address { get; set; }
    public string? city { get; set; }
    public string? region { get; set; }
    public string? postalcode { get; set; }
    public string? country { get; set; }
    public string? homephone { get; set; }
    public string? extension { get; set; }
    public string? photo { get; set; }
    public string? notes { get; set; }
    public string? reportsto { get; set; }
    public string? photopath { get; set; }

    public Employee(string[] args) {
        employeeid = args[0];
        lastname = args[1];
        firstname = args[2];
        title = args[3];
        titleofcourtesy = args[4];
        birthdate = args[5];
        hiredate = args[6];
        address = args[7];
        city = args[8];
        region = args[9];
        postalcode = args[10];
        country = args[11];
        homephone = args[12];
        extension = args[13];
        photo = args[14];
        notes = args[15];
        reportsto = args[16];
        photopath = args[17];
    }
}