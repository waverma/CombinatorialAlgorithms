using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace urfu_combinatorial_algorithms
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

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString() => (X + 1).ToString() + (Y + 1).ToString();
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
    
    public class Task2 : ITask<List<string>, MazeWithTarget>
    {
        public MazeWithTarget LoadFromFile(string filePath)
        {
            var data = File.ReadAllLines(filePath);
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
            var checkedNodes = new HashSet<Point>();
            var queue = new Queue<Point>();
            var previousPoint = new Point(-1, -1);
            var transitions = new Dictionary<Point, Point>();

            queue.Enqueue(inputData.Target);
            while (queue.Any())
            {
                var currentPoint = queue.Dequeue();
                transitions[currentPoint] = previousPoint;
                if (checkedNodes.Contains(currentPoint))
                    continue;
                checkedNodes.Add(currentPoint);
                
                
                if (inputData.Start.Equals(currentPoint))
                {
                    var result = new List<string> {"Y"};
                    var currentPointToFindPath = inputData.Start;
                    while (!currentPointToFindPath.Equals(inputData.Target))
                    {
                        result.Add(currentPointToFindPath.ToString());
                        currentPointToFindPath = transitions[currentPointToFindPath];
                    }
                    result.Add(inputData.Target.ToString());
                    return result;
                }

                
                foreach (var nextPoint in GetIncidentPoints(inputData.Maze, currentPoint))
                    if (!inputData.Maze[nextPoint.X, nextPoint.Y] && !checkedNodes.Contains(nextPoint))
                        queue.Enqueue(nextPoint);
                previousPoint = currentPoint;
            }

            return new List<string> {"N"};
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