using System.Data;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace tweets {
    public class TweetsList {
        public List<Tweet>? data { get; set; }
        string format = "MMMM dd, yyyy 'at' hh:mmtt";
        public TweetsList() {
            data = new List<Tweet>();
        }

        public void Print() {
            foreach(Tweet t in data)
                System.Console.WriteLine(t.ToString());
        }

        public void ReadFromJson(string? filename) {
            if (!File.Exists(filename))
                throw new FileNotFoundException();
            
            string jsonString = File.ReadAllText("data.json");  
            TweetsList ?list = JsonSerializer.Deserialize<TweetsList>(jsonString);
            this.data = list?.data;
        }

        public void WriteToXml(string? filename) {
            XmlSerializer x = new XmlSerializer(typeof(List<Tweet>));
            using (StreamWriter writer = new StreamWriter(filename))
            {
                x.Serialize(writer, data);
            }
        }

        public void ReadFromXml(string? filename) {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            XmlSerializer x = new XmlSerializer(typeof(List<Tweet>));
            using (StreamReader reader = new StreamReader(filename))
            {
                data = (List<Tweet>)x.Deserialize(reader);
            }
        }

        public void SortByUsers() {
            data?.Sort((t1, t2) => t1.UserName.CompareTo(t2.UserName)); 
        }

        public void SortByDate() {
            var provider = CultureInfo.InvariantCulture;
            data?.Sort((t1, t2) => DateTime.Compare(DateTime.ParseExact(t1.CreatedAt, format, provider), DateTime.ParseExact(t2.CreatedAt, format, provider)));
        }

        public void SortByUsersAndDate() {
            var provider = CultureInfo.InvariantCulture;
            data = data?.OrderBy(t => t.UserName).ThenBy(t => DateTime.ParseExact(t.CreatedAt, format, provider)).ToList();
        }

        public Tweet NewestTweet() {
            var provider = CultureInfo.InvariantCulture;
            return data.OrderBy(t => DateTime.ParseExact(t.CreatedAt, format, provider)).Last();
        }

        public Tweet OldestTweet() {
            var provider = CultureInfo.InvariantCulture;
            return data.OrderBy(t => DateTime.ParseExact(t.CreatedAt, format, provider)).First();
        }

        public Dictionary<String, List<Tweet>> TweetsByUsers() {
            Dictionary<String, List<Tweet>> res = new Dictionary<string, List<Tweet>>();
            foreach(Tweet t in data) {
                if(!res.ContainsKey(t.UserName))
                    res.Add(t.UserName, new List<Tweet> {t});
                else
                    res[t.UserName].Add(t);
            }
            return res;
        }

        public Dictionary<string, int> FrequencyOfWords() {
            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach(Tweet t in data) {
                string[] words = t.Text.Split(' ', '.', ',', '!', '?', ':', ';', '-', '(', ')', '[', ']', '{', '}', '/', '\\', '"', '\'', '\t', '\n', '\r');
                foreach(string w in words) {
                    string w_to_lower = w.ToLower();
                    if(!res.ContainsKey(w_to_lower))
                        res.Add(w_to_lower, 1);
                    else
                        res[w_to_lower]++;
                }
            }
            return res.OrderByDescending(word => word.Value).ToDictionary();
        }

        public Dictionary<string, int> Top10FrequentWordsToDict() {
            Dictionary<string, int> frequency = FrequencyOfWords();
            return frequency
                .Where(kvp => kvp.Key.Length >= 5)
                .Take(10)
                .ToDictionary();
        }

        public Dictionary<string, double> Idf() {
            // slownik <slowo, ilosc tweetow w ktorych to slowo wystepuje>
            Dictionary<string, int> res = new Dictionary<string, int>();

            foreach(Tweet t in data) {
                string[] words = t.Text.Split(' ', '.', ',', '!', '?', ':', ';', '-', '(', ')', '[', ']', '{', '}', '/', '\\', '"', '\'', '\t', '\n', '\r');
                List<string> occured_word = new List<string>();

                foreach(string w in words) {
                    string w_to_lower = w.ToLower();
                    if(!occured_word.Contains(w_to_lower)) {
                        if(!res.ContainsKey(w_to_lower))
                            res.Add(w_to_lower, 1);
                        else
                            res[w_to_lower]++;

                        occured_word.Add(w_to_lower);
                    }
                }
            }
            // slownik <slowo, obliczone idf>
            Dictionary<string, double> result = new Dictionary<string, double>();

            foreach(KeyValuePair<string, int> kvp in res) {
                double idf = Math.Log(data.Count / kvp.Value);
                result.Add(kvp.Key, idf);
            }

            return result.OrderByDescending(key => key.Value).Take(10).ToDictionary();
        }
    }

}