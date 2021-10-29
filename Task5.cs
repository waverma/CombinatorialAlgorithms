using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CombinatorialAlgorithms
{
    public class BipartiteGraph
    {
        public bool[][] Matrix { get; }
        public Func<int, int[]> EMaker { get; }
        public int XCount { get; } 
        public int YCount {get;}

        public BipartiteGraph(int xCount, int yCount, int[][] adjacencyLists)
        {
            XCount = xCount;
            YCount = yCount;
            Matrix = new bool[xCount][];

            for (var currentX = 0; currentX < adjacencyLists.Length; currentX++)
            {
                Matrix[currentX] = new bool[yCount];
                var adjacencyList = adjacencyLists[currentX];
                foreach (var node in adjacencyList)
                    Matrix[currentX][node] = true;
            }
            
            EMaker = x => Matrix[x]
                .Select((edgeFlag, y) => (y, edgeFlag))
                .Where(y => y.edgeFlag)
                .Select(y => y.y)
                .ToArray();
        }
    }
    
    public class Task5 : ITask, ITask<List<int>, BipartiteGraph>
    {
        private int[] currentBipartiteX;
        private int[] currentBipartiteY;

        public void Solve(string inputFilePath, string outputFilePath)
        {
            SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
        }

        public BipartiteGraph LoadFromFile(string filePath)
        {
            var data = File.ReadLines(filePath).ToArray();
            var xAndYCount = data.First().Split(' ').Select(int.Parse).ToArray();

            var matrix = data
                .Skip(1)
                .Select(x => x.Split(' ')
                                   .Select(int.Parse)
                                   .Select(y => y - 1)
                                   .Where(y => y >= 0)
                                   .ToArray()
                ).ToArray();

            return new BipartiteGraph(xAndYCount[0], xAndYCount[1], matrix);
        }

        public void SaveToFile(List<int> data, string path)
        {
            File.WriteAllText(path, string.Join(" ", data.Select(x => (x + 1).ToString()).ToArray()));
        }

        public List<int> Solve(BipartiteGraph bipartiteGraph)
        {
            currentBipartiteX = new int[bipartiteGraph.XCount].Select(v => -1).ToArray();
            currentBipartiteY = new int[bipartiteGraph.YCount].Select(v => -1).ToArray();
            
            var isPath = true;
            while (isPath)
            {
                isPath = false;
                for (var x = 0; x < bipartiteGraph.XCount; x++)
                {
                    if (currentBipartiteX[x] == -1 && DFS(x, bipartiteGraph.EMaker, new HashSet<int>())) isPath = true;
                }
            }
            
            return currentBipartiteX.ToList();
        }

        private bool DFS(int startXVertex, Func<int, int[]> eMaker, HashSet<int> visitedVertex)
        {
            if (visitedVertex.Contains(startXVertex)) return false;
            visitedVertex.Add(startXVertex);
            
            foreach (var y in eMaker(startXVertex))
            {
                if (currentBipartiteY[y] == -1)
                {
                    currentBipartiteY[y] = startXVertex;
                    currentBipartiteX[startXVertex] = y;
                    return true;
                }
                if (DFS(currentBipartiteY[y], eMaker, visitedVertex))
                {
                    currentBipartiteY[y] = startXVertex;
                    currentBipartiteX[startXVertex] = y;
                    return true;
                }
            }

            return false;
        }
    }
}