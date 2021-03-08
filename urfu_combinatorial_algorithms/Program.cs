using System;
using System.Collections.Generic;
using System.Linq;

namespace CombinatorialAlgorithms
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var inputFileName = @"..\..\input2.txt";
            var outputFileName = @"..\..\output2.txt";
            var taskNumber = 1;
            
            var tasksContainer = new List<Lazy<ITask>>
            {
                new Lazy<ITask>(() => new Task1()),
                new Lazy<ITask>(() => new Task2()),
            };

            if (!(args is null) && args.Length > 0)
            {
                var parsedArg = string.Join(" ", args)
                    .Split('\'')
                    .Where(x => x != " ")
                    .Where(x => x != "")
                    .ToArray();
                
                if (parsedArg.Length != 3) throw new ArgumentException(string.Join(" ", args));

                inputFileName = parsedArg[0];
                outputFileName = parsedArg[1];
                taskNumber = int.Parse(parsedArg[2]);
            }

            tasksContainer[taskNumber].Value.Solve(inputFileName, outputFileName);
        }
    }
}