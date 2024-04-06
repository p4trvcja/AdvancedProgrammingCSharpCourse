public class Program {
    public static void Main(string[] args) {
        DirectoryWatcher directoryWatcher = new DirectoryWatcher("C:\\Users\\Patrycja\\OneDrive\\studia\\semestr4\\ProgramowanieZaawansowane2\\lab5\\Directory_Watcher");
        Thread thread = new Thread(new ThreadStart(directoryWatcher.Start));
        thread.Start();
    }
}