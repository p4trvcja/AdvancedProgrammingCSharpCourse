public class Program {
    public static void Main(string[] args) {
        long numberOfThreads = long.Parse(args[0]);
        Threads t = new Threads(numberOfThreads);
        t.Start();
        Console.WriteLine("End of the main thread.");
    }
}
