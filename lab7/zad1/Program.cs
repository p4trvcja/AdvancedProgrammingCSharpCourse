using System.Security.Cryptography;
using System.Text;

public class Program {
    public static void Main(string[] args) {
        if(args.Length == 0) {
            Console.WriteLine("Invalid size of parameters. Valid commands:");
            Console.WriteLine("- dotnet run 0 - generates private and public keys");
            Console.WriteLine("- dotnet run 1 [input_file] [encrypted_file] - encrypts the input_file and saves the result to the encrypted_file");
            Console.WriteLine("- dotnet run 2 [encrypted_file] [decrypted_file] - decrypts the encrypted_file and saves the result to the decrypted_file");
            return;
        }
        if(!Int32.TryParse(args[0], out int commandType)) {
            Console.WriteLine("Invalid command type. It should be a number.");
            return;
        }

        switch(commandType) {
            case 0:
                GenerateKeys();
                break;
            case 1:
                if(args.Length != 3) {
                    Console.WriteLine("Invalid size of parameters. Try: dotnet run 1 [input_file] [encrypted_file]");
                    return;
                }
                if(!File.Exists(args[1])) {
                    Console.WriteLine("The input file does not exist.");
                    return;
                }
                EncryptFile(args[1], args[2]);
                break;
            case 2:
                if(args.Length != 3) {
                    Console.WriteLine("Invalid size of parameters. Try: dotnet run 2 [encrypted_file] [decrypted_file]");
                    return;
                }
                if(!File.Exists(args[1])) {
                    Console.WriteLine("The input file does not exist.");
                    return;
                }
                DecryptFile(args[1], args[2]);
                break;
            default:
                Console.WriteLine("Incorrect command type. Valid command types: [0 | 1 | 2]");
                break;
        }
    }
    private static void GenerateKeys() {
        string filePublicKey = "publicKey.dat";
        string filePrivateKey = "privateKey.dat";
        string ?publicKey = null;
        string ?privateKey = null;

        using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
            if (File.Exists(filePublicKey))
                publicKey = File.ReadAllText(filePublicKey);
            else {
                publicKey = rsa.ToXmlString(false);
                File.WriteAllText(filePublicKey, publicKey);
                Console.WriteLine("Public key generated.");
            }

            if (File.Exists(filePrivateKey))
                privateKey = File.ReadAllText(filePrivateKey);
            else {
                privateKey = rsa.ToXmlString(true);
                File.WriteAllText(filePrivateKey, privateKey);   
                Console.WriteLine("Private key generated.");
            }
        }
    }
    private static void EncryptFile(string inputFile, string outputFile) {
        string publicKey = File.ReadAllText("publicKey.dat");

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) { 
            rsa.FromXmlString(publicKey);
            byte[] dataToEncrypt = File.ReadAllBytes(inputFile);
            byte[] encryptedData = rsa.Encrypt(dataToEncrypt, false);
            File.WriteAllBytes(outputFile, encryptedData);
        }
        Console.WriteLine("Data has been encrypted.");
    }

    private static void DecryptFile(string inputFile, string outputFile) {
        string privateKey = File.ReadAllText("privateKey.dat");

        using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
            rsa.FromXmlString(privateKey);
            byte[] dataToDecrypt = File.ReadAllBytes(inputFile);
            byte[] decryptedData = rsa.Decrypt(dataToDecrypt, false);
            File.WriteAllBytes(outputFile, decryptedData);
        }
        Console.WriteLine("Data has been decrypted.");
    }
}