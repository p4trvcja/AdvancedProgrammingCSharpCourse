public class Program {
    public static void Main(string[] args) {
        DirectoryWatcher directoryWatcher = new DirectoryWatcher(".\\");
        Thread thread = new Thread(new ThreadStart(directoryWatcher.Start));
        thread.Start();
    }
}
