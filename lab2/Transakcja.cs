namespace bank {
    public class Transakcja {
            private RachunekBankowy? rachunekZrodlowy;
            private RachunekBankowy? rachunekDocelowy;
            private decimal kwota;
            private string? opis;
            public RachunekBankowy? RachunekZrodlowy{ get => rachunekZrodlowy; set => rachunekZrodlowy = value; }
            public RachunekBankowy? RachunekDocelowy{ get => rachunekDocelowy; set => rachunekDocelowy = value; }
            public decimal Kwota{ get => kwota; set => kwota = value; }
            public string? Opis { get => opis ?? ""; set => opis = value; }

            public Transakcja(RachunekBankowy rachunekZrodlowy_, RachunekBankowy rachunekDocelowy_, decimal kwota_, string opis_){
                if(rachunekZrodlowy_ == null && rachunekDocelowy_== null)
                    throw new Exception("Rachunek Bankowy i Rachunek Docelowy jest null");
                
                RachunekZrodlowy = rachunekZrodlowy_;
                RachunekDocelowy = rachunekDocelowy_;
                Kwota = kwota_;
                Opis = opis_;
            }

            override public String ToString(){
                return "Transakcja: " + "\n" + "RachunekZrodlowy: " + RachunekZrodlowy?.Numer + 
                ", RachunekDocelowy "+ RachunekDocelowy?.Numer + " Kwota: " + Kwota + " Opis: "+ Opis;
            }
    }
}