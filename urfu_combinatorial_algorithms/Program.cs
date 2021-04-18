using System;
using System.Collections.Generic;
using System.Linq;

namespace CombinatorialAlgorithms
{
    internal class Program
    {
        private static int TASK_NUMBER = 1;
        private const string IN_FILE_NAME = @"..\..\input2.txt";
        private const string OUT_FILE_NAME = @"..\..\output2.txt";
        
        public static void Main(string[] args)
        {
            if (args.Length == 1 && int.TryParse(args[0], out var result)) TASK_NUMBER = result;
            
            var tasksContainer = new List<Lazy<ITask>>
            {
                new Lazy<ITask>(() => new Task1()),
                new Lazy<ITask>(() => new Task2()),
            };

            // if (!(args is null) && args.Length > 0)
            // {
            //     var parsedArg = string.Join(" ", args)
            //         .Split('\'')
            //         .Where(x => x != " ")
            //         .Where(x => x != "")
            //         .ToArray();
            //     
            //     if (parsedArg.Length != 3) throw new ArgumentException(string.Join(" ", args));
            //
            //     inputFileName = parsedArg[0];
            //     outputFileName = parsedArg[1];
            //     taskNumber = int.Parse(parsedArg[2]);
            // }

            tasksContainer[TASK_NUMBER].Value.Solve(IN_FILE_NAME, OUT_FILE_NAME);
        }
    }
}