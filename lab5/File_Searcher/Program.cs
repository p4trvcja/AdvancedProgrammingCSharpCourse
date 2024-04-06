public class Program {
    public static void Main(string[] args) {
        FileSearch search = new FileSearch(".\\", "test", PrintFileName);
        Thread threadSearch = new Thread(new ThreadStart(search.Start));
        threadSearch.Start();
    }

    public static void PrintFileName(string fileName) {
        Console.WriteLine("Found file: " + fileName);
    }
}