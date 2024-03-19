namespace bank {
    public class Program {
        public static void Main(string[] args) {
            OsobaFizyczna osobaFizyczna = new OsobaFizyczna("osoba", "fizyczna", "", "11111111111", "");
            OsobaPrawna osobaPrawna = new OsobaPrawna("osoba", "prawna");

            RachunekBankowy rachunekBankowy = new RachunekBankowy("123", 15_000, true, new List<PosiadaczRachunku>{osobaFizyczna});
            RachunekBankowy rb = new RachunekBankowy("987", 350_000, false, new List<PosiadaczRachunku>{osobaPrawna});

            System.Console.WriteLine(rachunekBankowy.ToString());

            System.Console.WriteLine("------------------------------------");

            RachunekBankowy.DokonajTransakcji(null, rachunekBankowy, 5000, "wplata gotowki");
            RachunekBankowy.DokonajTransakcji(rb, rachunekBankowy, 4300, "wyplata miesieczna");

            System.Console.WriteLine(rachunekBankowy.ToString());

            System.Console.WriteLine("------------------------------------");

            rachunekBankowy += osobaPrawna;
            foreach(PosiadaczRachunku p in rachunekBankowy.PosiadaczeRachunku)
                System.Console.WriteLine(p.ToString());

            System.Console.WriteLine("------------------------------------");

            try {
                rb -= osobaPrawna;
            } catch(Exception e) {
                System.Console.WriteLine("Zgodnie z przewidywaniem wykryto błąd: " + e.Message);
            }
        }
    }
}