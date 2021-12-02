using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CombinatorialAlgorithms
{
    public class MyEdge
    {
        public Point From { get; }
        public Point To { get; }
        public int Size => From.GetMetricTo(To); 

        public MyEdge(Point from, Point to)
        {
            if (from == to) throw new Exception($"Edge connect similar points: {from.ToString()}");
            From = from;
            To = to;
        }
    }
    
    public class GrowingUp
    {
        public Point[] Source { get; }
        public Dictionary<Point, Point> Name { get; } = new Dictionary<Point, Point>();
        public Dictionary<Point, Point> Next { get; } = new Dictionary<Point, Point>();
        public Dictionary<Point, int> Size { get; } = new Dictionary<Point, int>();
        public List<MyEdge> Edges { get; } = new List<MyEdge>();
        
        public GrowingUp(IEnumerable<Point> points)
        {
            Source = points.ToArray();
            foreach (var point in Source)
            {
                Name[point] = point;
                Size[point] = 1;
                Next[point] = point;
            }

            var temp = Source.ToHashSet();
            while (temp.Any())
            {
                var rndPoint = temp.First();
                foreach (var point in temp.Where(x => x != rndPoint))
                    Edges.Add(new MyEdge(rndPoint, point));
                temp.Remove(rndPoint);
            }
        }

        public void Join(Point v, Point w, Point p, Point q)
        {
            Name[w] = p;
            var u = Next[w];
            while (Name[u] != p)
            {
                Name[u] = p;
                u = Next[u];
            }

            Size[p] += Size[q];
            var x = Next[v];
            var y = Next[w];
            Next[v] = y;
            Next[w] = x;
        }
    }

    public class Alg
    {
        private readonly GrowingUp growingUp;

        private Point[] Source => growingUp.Source;
        private Dictionary<Point, Point> Name => growingUp.Name;
        private Dictionary<Point, Point> Next => growingUp.Next;
        private Dictionary<Point, int> Size => growingUp.Size;
        private IEnumerable<MyEdge> Edges => growingUp.Edges;
        
        public Alg(GrowingUp growingUp)
        {
            this.growingUp = growingUp;
        }
        
        public HashSet<MyEdge> Solve()
        {
            var queue = new Queue<MyEdge>();
            var result = new HashSet<MyEdge>();
            foreach (var edge in Edges.OrderBy(x => x.Size))
                queue.Enqueue(edge);

            while (result.Count != Source.Length - 1)
            {
                var vw = queue.Dequeue();
                // _ = queue.Dequeue();
                var p = Name[vw.From];
                var q = Name[vw.To];

                if (p != q)
                {
                    if (Size[p] > Size[q])
                        growingUp.Join(vw.From, vw.To, p, q);
                    else
                        growingUp.Join(vw.To, vw.From, q, p);

                    result.Add(vw);
                }
            }
            
            return result;
        }
    }
    
    public class Task4 : ITask, ITask<(List<List<string>>, int), List<Point>>
    {
        public void Solve(string inputFilePath, string outputFilePath)
        {
            SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
        }

        public List<Point> LoadFromFile(string filePath)
        {
            return File.ReadAllLines(filePath)
                .Where(x => x != "")
                .Skip(1)
                .Select(x => new Point(x))
                .ToList();
        }

        public void SaveToFile((List<List<string>>, int) data, string path)
        {
            var input = LoadFromFile("in.txt");
            var result = data.Item1.Select((x, i) => string.Join(" ", x.OrderBy(y => y)) + " 0").ToList();
            // var result = data.Item1.Select((x, i) => $"({input[i].ToString()}):\t\t{string.Join("\t\t", x.OrderBy(y => y).Select(y => $"({input[int.Parse(y) - 1]})"))} 0").ToList();
            result.Add(data.Item2.ToString());
            File.WriteAllLines(path, result);
        }

        public (List<List<string>>, int) Solve(List<Point> inputData)
        {
            var resultOfAlg = new Alg(new GrowingUp(inputData)).Solve();
            var a = resultOfAlg.Concat(resultOfAlg.Select(x => new MyEdge(x.To, x.From)))
                .GroupBy(x => x.From.Id)
                .OrderBy(x => x.Key)
                // .Select((x, i) => x.Select(y => $"({i} - - {x.Key} - - {y.To.Id})").ToList())
                .Select(x => x.Select(y => y.To.Id.ToString()).ToList())
                .ToList();
            // var b = a.GroupBy(x => x.Item1.Id)
            //     .Select(x => x.Select(y => y.Item2.Id).ToList())
            //     .ToList();
            
            return (a, resultOfAlg.Sum(x => x.Size));
        }
    }
}