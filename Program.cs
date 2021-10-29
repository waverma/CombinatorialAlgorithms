using System;
using System.Collections.Generic;

namespace CombinatorialAlgorithms
{
    internal static class Program
    {
        private const int TaskNumber = 5;
        private static readonly (string, string) DebugFilePaths = (@"..\..\in.txt", @"..\..\out.txt");
        private static readonly (string, string) ReleaseFilePaths = (@"in.txt", @"out.txt");
        private static bool debugMode;
        
        public static void Main()
        {
            debugMode = false;
            
            var tasksContainer = new List<Lazy<ITask>>
            {
                new Lazy<ITask>(() => new Task1()),
                new Lazy<ITask>(() => new Task2()),
                new Lazy<ITask>(() => new Task3()),
                new Lazy<ITask>(() => new Task4()),
                new Lazy<ITask>(() => new Task5()),
            };

            var inFileName = debugMode ? DebugFilePaths.Item1 : ReleaseFilePaths.Item1;
            var outFileName = debugMode ? DebugFilePaths.Item2 : ReleaseFilePaths.Item2;
            tasksContainer[TaskNumber - 1].Value.Solve(inFileName, outFileName);
        }
    }
}