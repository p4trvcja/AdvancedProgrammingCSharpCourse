public class EmployeeTerritory {
    public string? employeeid { get; set; }
    public string? territoryid { get; set; }

    public EmployeeTerritory(string employeeid_, string territoryid_) {
        employeeid = employeeid_;
        territoryid = territoryid_;
    }
}