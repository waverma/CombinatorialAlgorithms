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
                    result[i, j] = line[j * 2] == '1';
            }

            return result;
        }

        public void SaveToFile(List<List<int>> data, string path)
        {
            var text = new List<string> {data.Count.ToString()};

            for (var i = 0; i < data.Count; i++)
            {
                text.Add(string.Join(" ", data[i].Select(x => x + 1)));
                if (i != data.Count - 1) text.Add("0");
            }
            
            File.WriteAllText(path, string.Join("\n", text));
        }
        
        public List<List<int>> Solve(bool[,] inputData)
        {
            var visited = new HashSet<int>();
            var connectivityComponents = new List<List<int>>();
            var nodeCount = inputData.GetLength(0);

            while (visited.Count < nodeCount)
            {
                var currentConnectivityComponent = new List<int>();
                
                foreach (var node in GetAllNodesByDfs(inputData, nodeCount, GetStartNode(visited, nodeCount)))
                {
                    currentConnectivityComponent.Add(node);
                    visited.Add(node);
                }
                
                connectivityComponents.Add(currentConnectivityComponent.OrderBy(x => x).ToList());
            }
            
            return connectivityComponents;
        }

        private IEnumerable<int> GetAllNodesByDfs(bool[,] inputData, int totalNodeCount, int startNode)
        {
            var stack = new Stack<int>();
            var visited = new HashSet<int>();
            
            stack.Push(startNode);
            while (stack.Any())
            {
                var currentNode = stack.Pop();
                if (visited.Contains(currentNode)) continue;
                yield return currentNode;
                visited.Add(currentNode);
                for (var i = 0; i < totalNodeCount; i++)
                    if (inputData[currentNode, i])
                        stack.Push(i);
            }
        }

        private int GetStartNode(HashSet<int> visited, int totalNodeCount)
        {
            var currentFather = -1;
            for (var i = 0; i < totalNodeCount; i++)
                if (!visited.Contains(i))
                {
                    currentFather = i;
                    break;
                }

            return currentFather;
        }

        public void Solve(string inputFilePath, string outputFilePath)
        {
            SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
        }
    }
}