public class FileSearch {
    public string searchPath;
    public string substring;
    public Action<string> FileFoundCallback; // delegate
    public FileSearch(string path_, string substring_, Action<string> callback_) {
        searchPath = path_;
        substring = substring_;
        FileFoundCallback = callback_;
    }
    public void Start() {
        Console.WriteLine("This thread is ready to start searching...");
        foreach (string filePath in Directory.GetFiles(searchPath, "*", SearchOption.AllDirectories)) {
            if (Path.GetFileName(filePath).Contains(substring)) {
                FileFoundCallback(filePath);
            }
        }
    }
}