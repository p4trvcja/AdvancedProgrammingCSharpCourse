public class Region {
    public string? regionid { get; set; }
    public string? regiondescription { get; set; }

    public Region(string regionid_, string regiondescription_) {
        regionid = regionid_;
        regiondescription = regiondescription_;
    }
}