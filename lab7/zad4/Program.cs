/*
Zaszyfrowanie pliku algorytmem klucza symetrycznego przy użyciu hasła. Program ma przyjmować cztery parametry: 
pliki (a), (b), hasło, typ operacji. 
    - Jeżeli typ operacji = 0 program ma zaszyfrować plik (a) algorytmem AES, którego klucz ma zostać 
    wygenerowany przy pomocy podanego hasła. Zaszyfrowane dane mają być zapisane do pliku (b).
    - Jeżeli typ operacji = 1 program ma odszyfrować plik (a) algorytmem AES, którego klucz ma zostać 
    wygenerowany przy pomocy podanego hasła. Odszyfrowane dane mają być zapisane do pliku (b).
Wszystkie dane wymagane do utworzenia klucza algorytmu AES z wyjątkiem hasła mogą byc wpisane "na sztywno" do programu.
*/

using System.Security.Cryptography;
using System.Text;

public class Program {
    public static void Main(string[] args) {
        if(args.Length < 4) {
            Console.Write("Invalid size of parameters. Please try again.\nValid command: ");
            Console.WriteLine("dotnet run [input_file] [output_file] [password] [0 | 1]");
            return;
        }
        if(!File.Exists(args[0])) {
            Console.WriteLine("Input file does not exist.");
            return;
        }

        string password = args[2];
        int commandType;
        if(!Int32.TryParse(args[3], out commandType) || (commandType != 0 && commandType != 1)) {
            Console.Write("Incorrect command type. Valid command types: [0 | 1]");
        }
        
        byte[] salt = Encoding.UTF8.GetBytes("RandomSaltValue");
        byte[] initVector = Encoding.UTF8.GetBytes("RandomInitVector");
        int iterations = 5;
        string text = File.ReadAllText(args[0]);

        if(commandType == 0) {
            string file = File.ReadAllText(args[0]);
            byte[] inputData = Encoding.UTF8.GetBytes(file);
            byte[] encrypted = Encrypt(password, salt, initVector, iterations, inputData);
            File.WriteAllBytes(args[1], encrypted);
            Console.WriteLine("Data encrypted successfully.");
        } else {
            byte[] file = File.ReadAllBytes(args[1]);
            byte[] decrypted = Decrypt(password, salt, initVector, iterations, file);
            string decryptedText =  Encoding.UTF8.GetString(decrypted);
            File.WriteAllBytes(args[1],decrypted);
            Console.WriteLine($"Original text: {text}");
            Console.WriteLine($"Decrypted text: {decryptedText}");
        }
    }

    public static byte[] Decrypt(string password, byte[]salt, byte[]initVector, int iterations, byte[] inputData) {
        Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        Aes aes = Aes.Create();
        aes.Key = k1.GetBytes(16);
        aes.IV = initVector;
        MemoryStream decryptionStreamBacking = new MemoryStream();
        CryptoStream decrypt = new CryptoStream(
            decryptionStreamBacking, aes.CreateDecryptor(), CryptoStreamMode.Write);
        decrypt.Write(inputData, 0, inputData.Length);
        decrypt.Flush();
        decrypt.Close();
        k1.Reset();
        return decryptionStreamBacking.ToArray();
    }

    public static byte[] Encrypt(string password, byte[]salt, byte[]initVector, int iterations, byte[] inputData) {
        Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        Aes aes = Aes.Create();
        aes.IV = initVector;
        aes.Key = k1.GetBytes(16);
        MemoryStream encryptionStream = new MemoryStream();
        CryptoStream encrypt = new CryptoStream(encryptionStream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
        encrypt.Write(inputData, 0, inputData.Length);
        encrypt.FlushFinalBlock();
        encrypt.Close();
        k1.Reset();
        return encryptionStream.ToArray();
    }
}