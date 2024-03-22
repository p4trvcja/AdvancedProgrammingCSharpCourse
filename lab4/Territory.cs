public class Territory {
    public string? territoryid { set; get; }
    public string? territorydescription { set; get; }
    public string? regionid { set; get; }

    public Territory(string territoryid_, string territorydescription_, string regionid_) {
        territoryid = territoryid_;
        territorydescription = territorydescription_;
        regionid = regionid_;
    }
}