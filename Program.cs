using System;
using System.Collections.Generic;
using System.Linq;
using urfu_combinatorial_algorithms;

namespace CombinatorialAlgorithms
{
    internal class Program
    {
        private static int TASK_NUMBER = 3;
        private const string IN_FILE_NAME = @"..\..\in.txt";
        private const string OUT_FILE_NAME = @"..\..\out.txt";
        
        public static void Main(string[] args)
        {
            
            var tasksContainer = new List<Lazy<ITask>>
            {
                new Lazy<ITask>(() => new Task1()),
                new Lazy<ITask>(() => new Task2()),
                new Lazy<ITask>(() => new Task3()),
                new Lazy<ITask>(() => new Task4()),
            };

            tasksContainer[TASK_NUMBER].Value.Solve(IN_FILE_NAME, OUT_FILE_NAME);
        }
    }
}