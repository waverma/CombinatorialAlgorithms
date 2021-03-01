namespace urfu_combinatorial_algorithms
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // var task1 = new Task1();
            // task1.SaveToFile(task1.Solve(task1.LoadFromFile(@"..\..\input.txt")), @"..\..\output.txt");
            
            var task2 = new Task2();
            task2.SaveToFile(task2.Solve(task2.LoadFromFile(@"..\..\input2.txt")), @"..\..\output2.txt");
        }
    }
}