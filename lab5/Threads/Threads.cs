public class SingleThread {
    public int threadId = 0;
    Threads parent;
    public SingleThread(Threads rodzic, int numer) {
        this.parent = rodzic;
        this.threadId = numer;
    }
    public void Start() {
        Thread.Sleep(100);
        System.Console.WriteLine($"Thread {threadId} started.");
        Interlocked.Increment(ref parent.activeThreads);
        Console.WriteLine($"Thread {threadId} is waiting for a signal.");
        Thread.Sleep(100);
        parent.ewhChild.WaitOne();
        Interlocked.Decrement(ref parent.activeThreads);
        
        parent.ewhParent.Set();
        Console.WriteLine($"Thread {threadId} finished working.");
    }
}
public class Threads {
    public long activeThreads = 0;
    public long numberOfThreads;
    
    public EventWaitHandle ewhChild;
    public EventWaitHandle ewhParent;
    public Threads(long numberOfThreads_) {
        numberOfThreads = numberOfThreads_;
        ewhChild = new EventWaitHandle(false, EventResetMode.AutoReset);
        ewhParent = new EventWaitHandle(false, EventResetMode.AutoReset);
    }

    public void Start() {
        for(int i = 0; i < numberOfThreads; i++) {
            SingleThread s = new SingleThread(this, i);
            Thread t = new Thread(new ThreadStart(s.Start));
            t.Start();
        }
       
        while(Interlocked.Read(ref activeThreads) != numberOfThreads) {
            Thread.Sleep(1000);
        }
        System.Console.WriteLine("All threads have started.");
        
        while(Interlocked.Read(ref activeThreads) > 0) {
            WaitHandle.SignalAndWait(ewhChild, ewhParent);
        }
    }
}
