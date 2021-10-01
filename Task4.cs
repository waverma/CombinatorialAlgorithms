using System.Collections.Generic;
using System.IO;
using System.Linq;
using CombinatorialAlgorithms;

namespace CombinatorialAlgorithms
{
    public class Edge
    {
        public Point From { get; }
        public Point To { get; }
        public int Size => From.GetMetricTo(To); 

        public Edge(Point from, Point to)
        {
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
        public List<Edge> Edges { get; } = new List<Edge>();
        
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
                    Edges.Add(new Edge(rndPoint, point));
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
        private IEnumerable<Edge> Edges => growingUp.Edges;
        
        public Alg(GrowingUp growingUp)
        {
            this.growingUp = growingUp;
        }
        
        public HashSet<Edge> Solve()
        {
            var queue = new Queue<Edge>();
            var result = new HashSet<Edge>();
            foreach (var edge in Edges.OrderBy(x => x.Size))
                queue.Enqueue(edge);

            while (result.Count != Source.Length - 1)
            {
                var vw = queue.Dequeue();
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
    
    public class Task4 : ITask, ITask<(List<List<int>>, int), List<Point>>
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
                // .Select(x => new Point(int.Parse(x.Split(' ')[0]), int.Parse(x.Split(' ')[1])))
                .Select(x => new Point(x))
                .ToList();
        }

        public void SaveToFile((List<List<int>>, int) data, string path)
        {
            var result = data.Item1.Select(x => string.Join(" ", x.OrderBy(y => y)) + " 0").ToList();
            result.Add(data.Item2.ToString());
            File.WriteAllLines(path, result);
        }

        public (List<List<int>>, int) Solve(List<Point> inputData)
        {
            var resultOfAlg = new Alg(new GrowingUp(inputData)).Solve();
            var a = resultOfAlg.Select(x => (x.From, x.To)).Concat(resultOfAlg.Select(x => (x.To, x.From))).GroupBy(x => x.Item1.Id).Select(x => x.Select(y => y.Item2.Id).ToList()).ToList();
            
            
            //var b = new Alg(new GrowingUp(inputData)).Solve().Select(x => (x.From, x.To)).Concat(resultOfAlg.Select(x => (x.To, x.From))).GroupBy(x => x.Item1.Id).Select(x => x.Select(y => y.Item2.Id).ToList()).ToList().Select(x => string.Join(" ", x.OrderBy(y => y)) + " 0").ToList();
            
            
            
            return (a, resultOfAlg.Sum(x => x.Size));
        }
    }
}