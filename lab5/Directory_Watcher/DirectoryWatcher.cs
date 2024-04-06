public class DirectoryWatcher {
    public string path;
    public DirectoryWatcher(string path_) {
        path = path_;
    }
    public void Start() {
        Console.WriteLine($"Monitoring directory: {path}");
        var watcher = new FileSystemWatcher(path) {
            NotifyFilter = NotifyFilters.FileName,

            IncludeSubdirectories = false,
            EnableRaisingEvents = true
        };

        watcher.Created += (sender, e) => Console.WriteLine($"Created file: {e.Name}");
        watcher.Deleted += (sender, e) => Console.WriteLine($"Deleted file: {e.Name}");

        Thread thread = new Thread(() => {
            Console.WriteLine("Press 'q' to quit");
            while(Console.ReadKey().KeyChar != 'q') {
                Console.WriteLine("Press 'q' to quit");
            }
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();

            Console.WriteLine("Stopped monitoring directory.");
        });
        thread.Start();
    }
}