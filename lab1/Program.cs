public class Program {
    public static void Task2() {
        System.Console.WriteLine("Enter some string. To exit insert mode, type 'koniec!'");

        StreamWriter sw = new StreamWriter("FileName.txt", append:true);
        string last = "";
        
        while(true) {
            string input = Console.ReadLine();
            if(input == "koniec!")
                break;
            sw.WriteLine(input);
            if(input.CompareTo(last) > 0)
                last = input;
        }
        sw.Close();

        if(last == "") 
            System.Console.WriteLine("No strings were entered");
        else 
            System.Console.WriteLine("Last string in lexicographic order: " + last);
    }


    public static void Task3(string[] args) {
        string filename = args[0];
        string str = args[1];

        if(!File.Exists(filename))
            throw new Exception("There is no such file!");
        
        StreamReader sr = new StreamReader(filename);
        int lineNumber = 0;

        while(!sr.EndOfStream) {
            string line = sr.ReadLine();
            lineNumber++;
            
            for (int index = 0;; index += str.Length) {
                index = line.IndexOf(str, index);
                if (index == -1) 
                    break;
                System.Console.WriteLine("line: " + lineNumber + ", index: " + index);
            }
        }

        sr.Close();
    }


    public static void Task4(string[] args) {
         string filename = args[0];
        int n = Int32.Parse(args[1]);

        int min = Int32.Parse(args[2]);
        int max = Int32.Parse(args[3]);

        int seed = int.Parse(args[4]);
        string numType = args[5];

        StreamWriter sw;

        if (File.Exists(filename))
            sw = new StreamWriter(filename, append: true);
        else
            sw = new StreamWriter(filename);

        Random random = new Random(seed);

        for(int i=0; i < n; i++) {
            if(numType == "integer")
                sw.WriteLine(random.Next(min,max));
            else if(numType == "real")
                sw.WriteLine(random.NextDouble() * (max-min) + min);
            else
                throw new Exception("There are no other choices");
        }
        sw.Close();
    }


    public static void Task5(string[] args) {
        string filename = args[0];

        if(!File.Exists(filename))
            throw new Exception("There is no such file");
        
        string[] lines = File.ReadAllLines(filename);
        int lineNumber = lines.Length;
        int charNumber = lines.Sum(s => s.Length);

        double[] numbers = lines.Select(Double.Parse).ToArray();
        double min = numbers.Min();        
        double max = numbers.Max();
        double avg = numbers.Average();
        
        Console.WriteLine($"Lines number: {lineNumber}");
        Console.WriteLine($"Char number: {charNumber}");
        Console.WriteLine($"Max number:: {max}");
        Console.WriteLine($"Min number: {min}");
        Console.WriteLine($"Average number: {avg}");
    }

    public static void Main(string[] args) {
        if(args.Length==0) {
            System.Console.WriteLine("Write 'dotnet run [task2 | task3 | task4 | task5] [some-arguments]'");
            return;
        }
        string taskNumber = args[0];
        switch (taskNumber.ToLower()) {
            case "task2":
                Task2();
                break;
            case "task3":
                Task3(args.Skip(1).ToArray());
                break;
            case "task4":
                Task4(args.Skip(1).ToArray());
                break;
            case "task5":
                Task5(args.Skip(1).ToArray());
                break;
            default:
                System.Console.WriteLine("Invalid task number");
                break;
        }
    }
}