using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;

public class Program {
    public List<Producer> producers;
    public List<Consumer> consumers;
    public List<DataItem> data;
    public List<Thread> threads;
    public int n;
    public int m;

    public Program(int n_, int m_) {
        producers = new List<Producer>();
        consumers = new List<Consumer>();
        data = new List<DataItem>();
        threads = new List<Thread>();
        n = n_;
        m = m_;
    }
    public void Start() {
        for(int i=0; i < n; i++) {
            Producer producer = new Producer(data, i, new Random().Next(100, 2000));
            // producer.Thread = new Thread(new ThreadStart(producer.Start));
            Thread t = new Thread(new ThreadStart(producer.Start));
            threads.Add(t);
            t.Start();
            producers.Add(producer);
            // producer.Thread.Start();
            System.Console.WriteLine("Producer" + producer.id + " started");
        }

        for(int i=0; i < m; i++) {
            Consumer consumer = new Consumer(data, i, new Random().Next(50, 1000));
            // consumer.Thread = new Thread(new ThreadStart(consumer.Start));
            Thread t = new Thread(new ThreadStart(consumer.Start));
            threads.Add(t);
            t.Start();
            consumers.Add(consumer);
            // consumer.Thread.Start();
            System.Console.WriteLine("Consumer" + consumer.id + " started");
        }
    } 
    public void Stop() {
        foreach(Producer p in producers) {
            p.running = false;
            System.Console.WriteLine("Producer " + p.id + " stopped");
        }
        foreach(Consumer c in consumers) {
            c.running = false;
            System.Console.WriteLine("Consumer" + c.id + " stopped");
        }

        foreach(Thread t in threads)
            t.Join();
    }

    public static void Main(string[] args) {
        Int32.TryParse(args[0], out int n);
        Int32.TryParse(args[1], out int m);   
        Program program = new Program(n, m);   
        program.Start();

        Console.WriteLine("Enter 'q' to quit");
        while(Console.ReadKey(true).KeyChar != 'q')
            Console.WriteLine("Enter 'q' to quit");
                
        program.Stop();
        System.Console.WriteLine("This is the end of a program");
    }
}