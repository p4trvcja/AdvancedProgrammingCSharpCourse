namespace bank {
    public class OsobaPrawna : PosiadaczRachunku {
        private string? nazwa;
        private string? siedziba;
    
        public string? Nazwa {get => nazwa;}
        public string? Siedziba {get => siedziba;}
    
        public OsobaPrawna(string nazwa_, string siedziba_) {
            this.nazwa = nazwa_;
            this.siedziba = siedziba_;
        }
    
        public override String ToString() {
            return "OsobaPrawna: " + Nazwa + " " + Siedziba;
        }
    }
}
