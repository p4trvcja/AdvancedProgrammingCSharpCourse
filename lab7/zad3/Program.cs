/*
Podpisywanie danych z pliku. Zakładamy, że mamy dwa pliki w których znajduje się klucz prywatny 
i publiczny algorytmu RSA. Te pliki zostały utworzone np. programem z punktu 1. Program pobiera 
nazwę dwóch plików (a) i (b). Program wczytuje plik (a). 

Jeśli plik (b) istnieje, program ma potraktować go jako podpis wygenerowany z pliku (a) przy pomocy 
klucza prywatnego. Program ma zweryfikować, czy podpis jest poprawny i wypisać wynik weryfikacji na ekran. 

Jeśli plik (b) nie istnieje, program ma wygenerować podpis danych z pliku (a) używając klucza prywatnego 
i zapisać ten podpis do pliku (b).
*/
using System.Security.Cryptography;

public class Program {
    public static void Main(string[] args) {
        if(args.Length < 2) {
            Console.WriteLine("Invalid number of parameters. Try: dotnet run [input_file] [output_file]");
            return;
        }
        if(!File.Exists(args[0])) {
            Console.WriteLine("Input file does not exist!");
            return;
        }

        using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
            string filePublicKey = "publicKey.dat";
            string filePrivateKey = "privateKey.dat";
            string ?publicKey = null;
            string ?privateKey = null;

            if (File.Exists(filePublicKey)) {
                publicKey = File.ReadAllText(filePublicKey);
            } else {
                publicKey = rsa.ToXmlString(false);
                File.WriteAllText(filePublicKey, publicKey);
            }
            if (File.Exists(filePrivateKey)) {
                privateKey = File.ReadAllText(filePrivateKey);
            } else {
                privateKey = rsa.ToXmlString(true);
                File.WriteAllText(filePrivateKey, privateKey);   
            }
        
            byte[] data = File.ReadAllBytes(args[0]);
            
            if(File.Exists(args[1])) {
                rsa.FromXmlString(privateKey);

                byte[] inputBytes = File.ReadAllBytes(args[0]);
                byte[] signatureBytes = File.ReadAllBytes(args[1]);

                bool verified = rsa.VerifyData(inputBytes, new SHA1CryptoServiceProvider(), signatureBytes);
                Console.WriteLine("Signature is: {0}", verified ? "valid" : "invalid");
            } else {
                rsa.FromXmlString(privateKey);

                byte[] inputBytes = File.ReadAllBytes(args[0]);
                byte[] signatureBytes = rsa.SignData(inputBytes, new SHA1CryptoServiceProvider());

                File.WriteAllBytes(args[1], signatureBytes);
                Console.WriteLine("Generated signature.");
            }
        }
    }
}