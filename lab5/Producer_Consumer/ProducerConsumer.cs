using System.Collections.Immutable;
using System.Data;
using System.Text;

public class DataItem {
    public int producerId { get; set; }

    public DataItem(int producerId_) {
        producerId = producerId_;
    }
}

public class Producer {
    public List<DataItem> data { get; set; }
    public int id;
    public int interval;
    public bool running = true;
    public Producer(List<DataItem> data_, int producerId_, int interval_) {
        data = data_;
        id = producerId_;
        interval = interval_;        
    }

    public void Start() {
        while(running) {
            Thread.Sleep(interval);
            produceData();
            // Console.WriteLine($"Producer {id} produced new element.");
        }
    }

    public void produceData() {
        lock(data) {
            data.Add(new DataItem(id));
        }
    }
}


public class Consumer {
    public List<DataItem> data { get; set; }
    public List<DataItem> consumed;
    public int id;
    public int interval;
    public bool running = true;
    public Consumer(List<DataItem> data_, int consumerId_, int interval_) {
        consumed = new List<DataItem>();
        data = data_;
        id = consumerId_;
        interval = interval_;
    }

    public void Start() {
        while(running) {
            Thread.Sleep(interval);
            consumeData();
        }
        Console.WriteLine($"Consumer {id} consumed data from: {toString()}");
    }

    public void consumeData() {
        lock(data) {
            if(data.Count > 0) {
                consumed.Add(data[0]);
                data.RemoveAt(0);
                                
                Console.WriteLine($"Consumer {id} consumed: {consumedToString()}");
            }
        }
    }

    public string consumedToString() {
        string allValues = "";
        foreach(DataItem item in consumed) {
            allValues += item.producerId + " ";
        }
        return allValues;
    }

    public string toString() {
        string res="\n";
        Dictionary<int, int> values = new Dictionary<int, int>();
        foreach(var c in consumed) {
            if(!values.ContainsKey(c.producerId))
                values.Add(c.producerId, 1);
            else
                values[c.producerId]++;
        }

        foreach(var kvp in values)
            res += "    Producer " + kvp.Key + " - " + kvp.Value + "\n";
            
        return res;
    }

}