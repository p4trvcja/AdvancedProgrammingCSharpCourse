/*
Program liczący sumę kontrolną. Napisz program który jako parametry przyjmuje nazwę pliku (a), 
nazwę pliku zawierającego hash (b) oraz algorytm hashowania (SHA256, SHA512 lub MD5) (c). 
Jeżeli plik hash (b) nie istnieje, program ma policzyć hash z pliku (a) i zapisać go pod nazwą (b). 
Jeżeli plik (b) istnieje, program ma zweryfikować hash i wypisać do konsoli, czy hash jest zgodny.
*/
using System.Security.Cryptography;
using System.Text;

public class Program {
    public static void Main(string[] args) {
        if(args.Length != 3) {
            Console.WriteLine("Invalid size of parameters. Valid command: [input_file] [hash_file] [SHA256 | SHA512 | MD5]");
            return;
        }
        if(!File.Exists(args[0])) {
            Console.WriteLine("Input file does not exist.");
            return;
        }
        if(!args[2].Equals("SHA256") && !args[2].Equals("SHA512") & !args[2].Equals("MD5")) {
            Console.WriteLine("Incorrect hash algorithm. Valid ones: [SHA256 | SHA512 | MD5]");
            return;
        }

        if(!File.Exists(args[1])) {
            string hash = ComputeHash(args[0], args[2]);
            File.WriteAllText(args[1], hash);
            Console.WriteLine("Hash file has been created.");
        } else {
            string storedHash = File.ReadAllText(args[1]);
            string computedHash = ComputeHash(args[0], args[2]);

            if (string.Equals(storedHash, computedHash, StringComparison.OrdinalIgnoreCase))
                Console.WriteLine("Hashes are matching.");
            else
                Console.WriteLine("Hashes do not match.");
        }
    }
    private static string ComputeHash(string inputFile, string algorithm) {
        using (var stream = File.OpenRead(inputFile)) {
            switch (algorithm) {
                case "SHA256":
                    using (var sha256 = SHA256.Create()) {
                        byte[] hash = sha256.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", string.Empty);
                    }
                case "SHA512":
                    using (var sha512 = SHA512.Create()) {
                        byte[] hash = sha512.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", string.Empty);
                    }
                case "MD5":
                    using (var md5 = MD5.Create()) {
                        byte[] hash = md5.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", string.Empty);
                    }
                default:
                    throw new ArgumentException("Invalid hash algorithm.");
            }
        }
    }
}