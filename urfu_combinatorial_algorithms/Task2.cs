using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CombinatorialAlgorithms
{
    public struct Point
    {
        public int X { get; }
        public int Y { get; }
        
        public Point(int x, int y)
        {
            X = x;    
            Y = y;
        }
        
        public int GetMetricTo(Point point)
        {
            return Math.Abs(X - point.X) + Math.Abs(Y - point.Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString() => (X) + ", " + (Y);
    }
    
    public class MazeWithTarget
    {
        public bool[,] Maze { get; }
        public Point Start { get; }
        public Point Target { get; }
        
        public MazeWithTarget(bool[,] maze, Point start, Point target)
        {
            Maze = maze;
            Start = start;
            Target = target;
        }
    }
    
    public class Task2 : ITask<List<string>, MazeWithTarget>, ITask
    {
        public void Solve(string inputFilePath, string outputFilePath) 
            => SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
        
        public MazeWithTarget LoadFromFile(string filePath)
        {
            var data = File.ReadAllLines(filePath).Where(x => x != "").ToArray();

            var maze = new bool[int.Parse(data[0]), int.Parse(data[1])];
            var start = new Point(
                int.Parse(data[int.Parse(data[0]) + 2].Split(' ')[0]) - 1,
                int.Parse(data[int.Parse(data[0]) + 2].Split(' ')[1]) - 1
                );
            var target = new Point(
                int.Parse(data[int.Parse(data[0]) + 3].Split(' ')[0]) - 1,
                int.Parse(data[int.Parse(data[0]) + 3].Split(' ')[1]) - 1
            );

            for (var i = 2; i < int.Parse(data[0]) + 2; i++)
            {
                var line = data[i].Split(' ');
                for (var j = 0; j < line.Length; j++)
                    maze[i - 2, j] = line[j] == "1";
            }
            
            return new MazeWithTarget(maze, start, target);
        }

        public void SaveToFile(List<string> data, string path)
        {
            File.WriteAllLines(path, data);
        }

        public List<string> Solve(MazeWithTarget inputData)
        {
            var visited = new HashSet<Point>();
            var previousPoint = new Point(-1, -1);
            var transitions = new Dictionary<Point, Point>();
            var queue = new Queue<Point>();

            queue.Enqueue(inputData.Target);
            while (queue.Any())
            {
                var currentPoint = queue.Dequeue();
                
                if (visited.Contains(currentPoint))
                    continue;
                visited.Add(currentPoint);
                
                
                if (inputData.Start.Equals(previousPoint))
                {
                    return GetResult(transitions, inputData.Start, inputData.Target);
                }


                var incident = GetIncidentPoints(inputData.Maze, currentPoint);
                foreach (var nextPoint in incident)
                    if (!inputData.Maze[nextPoint.X, nextPoint.Y] && !visited.Contains(nextPoint))
                    {
                        transitions[nextPoint] = currentPoint;
                        queue.Enqueue(nextPoint);
                    }
                previousPoint = currentPoint;
            }

            return new List<string> {"N"};
        }

        private static List<string> GetResult(IReadOnlyDictionary<Point, Point> transitions, Point start, Point target)
        {
            var result = new List<string> {"Y"};
            var currentPointToFindPath = start;
            while (!currentPointToFindPath.Equals(target))
            {
                result.Add(currentPointToFindPath.ToString());
                if (!transitions.ContainsKey(currentPointToFindPath)) break;
                currentPointToFindPath = transitions[currentPointToFindPath];
            }
            result.Add(target.ToString());
            return result;
        }

        private static IEnumerable<Point> GetIncidentPoints(bool[,] maze, Point point)
        {
            if (point.Y > 0) yield return new Point(point.X, point.Y - 1);
            if (point.Y < maze.GetLength(1) - 1) yield return new Point(point.X, point.Y + 1);
            if (point.X > 0) yield return new Point(point.X - 1, point.Y);
            if (point.X < maze.GetLength(0) - 1) yield return new Point(point.X + 1, point.Y);
        }
    }
}