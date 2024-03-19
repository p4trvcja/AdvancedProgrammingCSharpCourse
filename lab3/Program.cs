using System.Text.Json;
using tweets;

public class Program {
    public static void Main(string[] args) {
        System.Console.WriteLine("------------------- Exercise 1 -------------------");

        TweetsList list = new TweetsList();
        list.ReadFromJson("data.json");
        list.Print();

        System.Console.WriteLine("------------------- Exercise 2 -------------------");
    
        list.WriteToXml("data.xml");
        TweetsList tweets = new TweetsList();
        tweets.ReadFromXml("data.xml");

        list.Print();
        
        System.Console.WriteLine("------------------- Exercise 3 -------------------");

        list.SortByUsersAndDate();
        foreach(Tweet t in list.data)
            System.Console.WriteLine(t.userAndDate());
        
        System.Console.WriteLine("------------------- Exercise 4 -------------------");

        System.Console.WriteLine("Newest tweet: " + list.NewestTweet().ToString());
        System.Console.WriteLine("Oldest tweet: " + list.OldestTweet().ToString());
        
        System.Console.WriteLine("------------------- Exercise 5 -------------------");

        Dictionary<String, List<Tweet>> tweetsByUsers = list.TweetsByUsers();
        foreach (KeyValuePair<string, List<Tweet>> kvp in tweetsByUsers) {
            System.Console.WriteLine("User: " + kvp.Key);
            foreach(Tweet t in kvp.Value)
                System.Console.WriteLine(t.ToString());
        }

        System.Console.WriteLine("------------------- Exercise 6 -------------------");

        Dictionary<string, int> res = list.FrequencyOfWords();
        foreach (KeyValuePair<string, int> kvp in res)
            System.Console.WriteLine("key: {0}, value: {1}", kvp.Key, kvp.Value);
        
        System.Console.WriteLine("------------------- Exercise 7 -------------------");

        Dictionary<string, int> topFrequentWords = list.Top10FrequentWordsToDict();
        foreach(KeyValuePair<string, int> kvp in topFrequentWords)
            System.Console.WriteLine("key: {0}, value: {1}", kvp.Key, kvp.Value);

        System.Console.WriteLine("------------------- Exercise 8 -------------------");

        Dictionary<string, double> idf = list.Idf();
        foreach(var i in idf)
            System.Console.WriteLine("key: {0}, idf: {1}", i.Key, i.Value);
    }
}