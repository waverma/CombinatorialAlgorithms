using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CombinatorialAlgorithms
{
    public enum STNodeType
    {
        S,
        T,
        X,
        Y
    }
    
    public class STNode
    {
        public IEnumerable<STNode> Next => next;
        private readonly List<STNode> next = new List<STNode>();
        public STNodeType STNodeType;
        public int Value;

        public STNode(STNodeType stNodeType, int value, params STNode[] nodes)
        {
            STNodeType = stNodeType;
            Value = value;
            foreach (var stNode in nodes)
                Add(stNode);
        }

        public void Add(STNode node)
        {
            next.Add(node);
        }
    }
    
    public class BipartiteGraph
    {
        public bool[][] Matrix { get; }
        public STNode STGraph { get; private set; }
        public int XCount => Matrix.GetLength(0); 
        public int YCount => Matrix.GetLength(1); 

        public BipartiteGraph(int xCount, int yCount, int[][] adjacencyLists)
        {
            Matrix = new bool[][xCount];

            for (var currentX = 0; currentX < adjacencyLists.Length; currentX++)
            {
                Matrix[currentX] = new bool[yCount];
                var adjacencyList = adjacencyLists[currentX];
                foreach (var node in adjacencyList)
                    Matrix[currentX][node] = true;
            }

            var tNode = new STNode(STNodeType.T, -2);
            
            var yNodes = new List<STNode>();
            for (var i = 0; i < yCount; i++) yNodes.Add(new STNode(STNodeType.Y, i, tNode));
            
            var xNodes = new List<STNode>();
            for (var i = 0; i < xCount; i++)
                xNodes.Add(new STNode(STNodeType.X, i, yNodes.Where(x => Matrix[i][x.Value]).ToArray()));

            STGraph = new STNode(STNodeType.S, -1, xNodes.ToArray());
        }
    }
    
    public class Task5 : ITask, ITask<List<int>, BipartiteGraph>
    {
        public void Solve(string inputFilePath, string outputFilePath)
        {
            SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
        }

        public BipartiteGraph LoadFromFile(string filePath)
        {
            var data = File.ReadLines(filePath).ToArray();
            var xAndYCount = data.First().Split(' ').Select(int.Parse).ToArray();

            var a = data
                .Skip(1)
                .Select(x => x.Split(' ').Select(int.Parse).ToArray())
                .ToArray();

            return new BipartiteGraph(xAndYCount[0], xAndYCount[1], a);
        }

        public void SaveToFile(List<int> data, string path)
        {
            throw new System.NotImplementedException();
        }

        public List<int> Solve(BipartiteGraph inputData)
        {
            throw new System.NotImplementedException();
        }

        
    }

    public class Algoritm
    {
        private int[] px;
        private int[] py;
        private bool[] vis;
        private BipartiteGraph bipartiteGraph;
        private bool isPath = true;

        public Algoritm()
        {
            bipartiteGraph = new BipartiteGraph()
            px = new int[bipartiteGraph.XCount];
            py = new int[bipartiteGraph.YCount];

            Fill(px, -1);
            Fill(py, -1);
        }

        private void Fill<T>(IList<T> array, T value)
        {
            for (var i = 0; i < array.Count; i++) array[i] = value;
        }

        public void Solve()
        {
            while (isPath)
            {
                isPath = false;
                Fill(vis, false);
                for (var x = 0; x < bipartiteGraph.XCount; x++)
                {
                    if (px[x] == -1 && DFS(x)) isPath = true;
                }
            }
        }
        
        private bool DFS(int x)
        {
            if (vis[x]) return false;
            vis[x] = true;

            foreach (var (_ y) in bipartiteGraph.Matrix[x].Select((k, i) => (k, i)).Where(k => k.k))
            {
                if (py[y] == -1)
                {
                    py[y] = x;
                    px[x] = y;
                    return true;
                }
                if (DFS(py[y]))
                {
                    py[y] = x;
                    px[x] = y;
                    return true;
                }
            }

            return false;
        }
    }
}