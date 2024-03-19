namespace bank {
    public class OsobaFizyczna : PosiadaczRachunku {
        private string? imie;
        private string? nazwisko;
        private string? drugieImie;
        private string? pesel;
        private string? numerPaszportu;
    
        public string? Imie { get => imie; set => imie = value; }
        public string? Nazwisko { get => nazwisko; set => nazwisko = value; }
        public string? DrugieImie { get => drugieImie; set => drugieImie = value; }
        public string? NumerPaszportu { get => numerPaszportu;  set => numerPaszportu = value; }
        public string? Pesel { 
            get => pesel;
            set {
                if(value != null && value.Length != 11) 
                    throw new Exception("Niepoprawna dlugosc PESEL");
                pesel = value;
            }
        }
    
        public OsobaFizyczna(string imie_, string nazwisko_, string drugieImie_, string pesel_, string numerPaszportu_) {
            if(pesel_ == null && numerPaszportu_ == null)
                throw new Exception("PESEL lub numerPaszportu nie moga byc null jednoczesnie");
    
            Imie = imie_;
            DrugieImie = drugieImie_;
            Nazwisko = nazwisko_;
            Pesel = pesel_;
            NumerPaszportu = numerPaszportu_;
        }
    
        public override String ToString() {
            return "OsobaFizyczna: " + Imie + " " + Nazwisko;
        }
    }
}
