using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CombinatorialAlgorithms
{
    public class Task1 : ITask<List<List<int>> , bool[,]>, ITask
    {
        public bool[,] LoadFromFile(string filePath)
        {
            var data = File.ReadLines(filePath).ToArray();
            var nodeCount = int.Parse(data[0]);
            var result = new bool[nodeCount, nodeCount];

            for (var i = 0; i < nodeCount; i++)
            {
                var line = data[i + 1];
                for (var j = 0; j < nodeCount; j++)
                    result[i, j] = line[j] == '1';
            }

            return result;
        }

        public void SaveToFile(List<List<int>> data, string path)
        {
            var text = new List<string> {data.Count.ToString()};

            for (var i = 0; i < data.Count; i++)
            {
                text.Add(string.Join("", data[i].Select(x => x + 1)));
                if (i != data.Count - 1) text.Add("0");
            }
            
            File.WriteAllText(path, string.Join("\n", text));
        }
        
        public List<List<int>> Solve(bool[,] inputData)
        {
            var checkedNodes = new HashSet<int>();
            var connectivityComponents = new List<List<int>>();

            while (checkedNodes.Count != inputData.GetLength(0))
            {
                var stack = new Stack<int>();
                var currentConnectivityComponent = new List<int>();
                var currentFather = -1;
                for (var i = 0; i < inputData.GetLength(0); i++)
                    if (!checkedNodes.Contains(i))
                    {
                        currentFather = i;
                        break;
                    }
                if (currentFather == -1) throw new Exception("currentFather == -1");
                
                stack.Push(currentFather);
                while (stack.Any())
                {
                    var currentNode = stack.Pop();
                    if (checkedNodes.Contains(currentNode))
                        continue;
                    currentConnectivityComponent.Add(currentNode);
                    checkedNodes.Add(currentNode);
                    for (var i = 0; i < inputData.GetLength(0); i++)
                        if (inputData[currentNode, i] &&  !checkedNodes.Contains(i))
                            stack.Push(i);
                }
                
                connectivityComponents.Add(currentConnectivityComponent);
            }
            
            return connectivityComponents;
        }

        public void Solve(string inputFilePath, string outputFilePath)
        {
            SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
        }
    }
}