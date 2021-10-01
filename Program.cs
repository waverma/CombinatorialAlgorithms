using System;
using System.Collections.Generic;

namespace CombinatorialAlgorithms
{
    internal static class Program
    {
        private const int TaskNumber = 4;
        // private const string InFileName = @"..\..\in.txt";
        // private const string OutFileName = @"..\..\out.txt";
        private const string InFileName = @"in.txt";
        private const string OutFileName = @"out.txt";
        
        public static void Main()
        {
            var tasksContainer = new List<Lazy<ITask>>
            {
                new Lazy<ITask>(() => new Task1()),
                new Lazy<ITask>(() => new Task2()),
                new Lazy<ITask>(() => new Task3()),
                new Lazy<ITask>(() => new Task4()),
            };

            tasksContainer[TaskNumber - 1].Value.Solve(InFileName, OutFileName);
        }
    }
}