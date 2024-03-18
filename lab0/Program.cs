public class Program {
    public static void Task1(string[] args) {
        if(!int.TryParse(args[args.Length-1], out int parameter)) {
            Console.WriteLine("The last parameter should be an integer.");
            return;
        }

        string res = string.Join(" ", args, 0, args.Length - 1);

        for(int i=0; i < parameter; i++) {
            Console.WriteLine(res);
        }
    }


    public static void Task2() {
        Console.WriteLine("Enter any quantity of numbers. Enter 0 to end.");

        StreamWriter sw;
        if(!File.Exists("File.txt"))
            sw = new StreamWriter("File.txt");
        else 
            sw = new StreamWriter("File.txt", append:true);

        double sum = 0;
        int nums = 0;

        bool flag = false;

        while(Double.TryParse(Console.ReadLine(), out double result)) {
            if(result == 0) {
                flag = true;
                break;
            }
            sum += result;
            nums++;
        }

        if(!flag) {
            Console.WriteLine("You didn't enter a number!");
            return;
        }

        double mean = sum / nums;

        sw.WriteLine("Sum = " + sum);        
        sw.WriteLine("Mean = " + mean);

        sw.Close();

        System.Console.WriteLine("Sum and mean of entered numbers saved to a file: File.txt");
    }


    public static void Task3(string[] args) {
        string filename = args[0];

        if(!File.Exists(filename)) {
            Console.WriteLine("File does not exist.");
            return;
        }

        string[] lines = File.ReadAllLines(filename);

        double[] numbers = lines.Select(double.Parse).ToArray();
        double max = numbers.Max();
        int line = Array.IndexOf(numbers, max);

        Console.WriteLine(max + ", line: " + (line+1));
    }


    public static void Task4(string[] args) {
        string[] sounds = ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "B", "H"];
        string startingSound = args[0].ToUpper();

        string[] result = new string[8];
        result[0] = startingSound;

        int index = Array.IndexOf(sounds, startingSound);

        for(int i=1; i < 8; i++) {
            if(i==3 || i == 7)
                index += 1;
            else
                index += 2;

            if(index > sounds.Length-1) index %= 12;
            result[i] = sounds[index];

        }

        foreach(string r in result) {
            Console.Write(r + " ");
        }
    }


    public static void Main(string[] args) {
        if(args.Length==0) {
            System.Console.WriteLine("Write 'dotnet run [task1 | task2 | task3 | task4] [some-arguments]'");
        }
        string taskNumber = args[0];

        switch (taskNumber.ToLower()) {
            case "task1":
                Task1(args.Skip(1).ToArray());
                break;
            case "task2":
                Task2();
                break;
            case "task3":
                Task3(args.Skip(1).ToArray());
                break;
            case "task4":
                Task4(args.Skip(1).ToArray());
                break;
            default:
                System.Console.WriteLine("Invalid task number");
                break;
        }
    }
}