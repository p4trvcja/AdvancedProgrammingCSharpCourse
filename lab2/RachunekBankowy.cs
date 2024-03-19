namespace bank {
    public class RachunekBankowy {
        private string? numer;
        private decimal? stanRachunku;
        private bool? czyDozwolonyDebet;
        private List<PosiadaczRachunku> posiadaczeRachunku = new List<PosiadaczRachunku>();
        private List<Transakcja> transakcje = new List<Transakcja>();

        public string? Numer {get => numer; set => numer = value;}
        public decimal? StanRachunku {get => stanRachunku; set => stanRachunku = value;}
        public bool? CzyDozwolonyDebet {get => czyDozwolonyDebet; set => czyDozwolonyDebet = value;}
        public List<PosiadaczRachunku> PosiadaczeRachunku { get => posiadaczeRachunku; set => posiadaczeRachunku = value; }
        public List<Transakcja> Transakcje { get => transakcje; set => transakcje = value; }

        public RachunekBankowy(string numer_, decimal stanRachunku_, bool czyDozwolonyDebet_, List<PosiadaczRachunku> posiadaczeRachunku_) {
            Numer = numer_;
            StanRachunku = stanRachunku_;
            CzyDozwolonyDebet = czyDozwolonyDebet_;
            PosiadaczeRachunku = posiadaczeRachunku_;

            if(posiadaczeRachunku.Count == 0)
                throw new Exception("Lista jest pusta");
        }

        public static void DokonajTransakcji(RachunekBankowy zrodlowy_, RachunekBankowy docelowy_, decimal kwota_, string opis_) {
            if(kwota_ < 0 || (zrodlowy_ == null && docelowy_ == null) || (zrodlowy_ != null && kwota_ > zrodlowy_.stanRachunku && zrodlowy_.czyDozwolonyDebet == false)) 
                throw new Exception("niespelnione wymagania, by dodac transakcje");

            if(zrodlowy_ == null) {
                docelowy_.stanRachunku += kwota_;
                docelowy_.transakcje.Add(new Transakcja(null, docelowy_, kwota_, opis_));
            }
            else if(docelowy_ == null) {
                zrodlowy_.stanRachunku -= kwota_;
                zrodlowy_.transakcje.Add(new Transakcja(zrodlowy_, null, kwota_, opis_));
            } else {
                zrodlowy_.stanRachunku -= kwota_;
                docelowy_.stanRachunku += kwota_;

                zrodlowy_.transakcje.Add(new Transakcja(zrodlowy_, docelowy_, kwota_, opis_));
                docelowy_.transakcje.Add(new Transakcja(zrodlowy_, docelowy_, kwota_, opis_));
            }

        }

        public static RachunekBankowy operator+ (RachunekBankowy one, PosiadaczRachunku other) {
            if(one.posiadaczeRachunku.Contains(other))
                throw new Exception("Posiadacz rachunku jest juz na liscie");
            
            one.posiadaczeRachunku.Add(other);
            return one;
        }

        public static RachunekBankowy operator- (RachunekBankowy one, PosiadaczRachunku other) {
            if(!one.posiadaczeRachunku.Contains(other))
                throw new Exception("Nie ma takiego posiadacza rachunku na liscie");
            else if(one.posiadaczeRachunku.Count == 1)
                throw new Exception("Liczba posiadaczy rachunku spadlaby ponizej 1");
            
            one.posiadaczeRachunku.Remove(other);
            return one;
        }

        public override String ToString() {
            string res = "numer rachunku: " + Numer + "\nstan rachunku: " + StanRachunku + "\n";
            foreach(PosiadaczRachunku p in posiadaczeRachunku) {
                res += p.ToString();
                res += "\n";
            }
            foreach(Transakcja t in transakcje) {
                res += t.ToString();
                res += "\n";
            }
            return res;
        }
    }
}