using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CombinatorialAlgorithms
{
	public static class IEnumerableExt
	{
		public static IEnumerable<T> Distinct<T, TValue>(this IEnumerable<T> source, Func<T, TValue> extractor)
		{
			var returned = new HashSet<TValue>();
			foreach (var value in source)
			{
				if (returned.Any(x => Equals(extractor(value), x))) continue;
				yield return value;
				returned.Add(extractor(value));
			}
		}
	}
	
    public class DijkstraData
    {
        public Node Previous { get; set; }
        public double Price { get; set; }
    }

    public class Node : IEnumerable<Node>
    {
	    public int Number { get; }
	    public List<(Node, double)> To { get; } = new List<(Node, double)>();
	    
	    public Node(int number)
	    {
		    Number = number;
	    }

	    private IEnumerable<Node> GetNodes()
	    {
		    yield return this;
		    foreach (var (node, _) in To)
		    {
			    yield return node;
			    foreach (var nextNode in node)
				    yield return nextNode;
		    }
	    }

	    public IEnumerator<Node> GetEnumerator() => GetNodes().Distinct(x => x.Number).GetEnumerator();

	    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Task6Info
    {
	    public int AllPathLength;
	    public int FuelTankVolume;
	    public int FuelPerLength;
	    public double StartSpentMoney;
	    public int FuelStationCount;
	    public IEnumerable<(int LengthFromStart, double FuelCost)> FuelStations;
    }
    
    public class Task6 : ITask<(double, List<Node>), Task6Info>, ITask
    {
	    public static (double, List<Node>) Dijkstra(Node graph, Node start, Node end)
	    {
		    var notVisited = graph.OrderBy(x => x.Number).ToList();
		    var track = new Dictionary<Node, DijkstraData>();
		    track[start] = new DijkstraData { Price = 0, Previous = null };

		    while (true)
		    {
			    Node toOpen = null;
			    var bestPrice = double.PositiveInfinity;
			    foreach (var e in notVisited.Where(e => track.ContainsKey(e) && track[e].Price < bestPrice))
			    {
				    bestPrice = track[e].Price;
				    toOpen = e;
			    }

			    if (toOpen == null) return (0, null);
			    if (toOpen.Number == end.Number) break;

			    foreach (var (nextNode, weight) in toOpen.To)
			    {
				    var currentPrice = track[toOpen].Price + weight;
				    if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice)
				    {
					    track[nextNode] = new DijkstraData { Previous = toOpen, Price = currentPrice };
				    }
			    }

			    notVisited.Remove(toOpen);
		    }

		    var result = new List<Node>();
		    while (end != null)
		    {
			    result.Add(end);
			    end = track[end].Previous;
		    }
		    result.Reverse();
		    return (track[result.Last()].Price, result);
	    }

	    public Task6Info LoadFromFile(string filePath)
	    {
		    var data = File.ReadAllLines(filePath);
		    var secondLine = data[1].Split(' ');
		    var result = new Task6Info
		    {
			    AllPathLength = int.Parse(data.First()),
			    FuelTankVolume = int.Parse(secondLine[0]),
			    FuelPerLength = int.Parse(secondLine[1]),
			    StartSpentMoney = double.Parse(secondLine[2], CultureInfo.InvariantCulture),
			    FuelStations = data.Skip(2).Select(x => (int.Parse(x.Split(' ').First(), CultureInfo.InvariantCulture) , double.Parse(x.Split(' ').Last(), CultureInfo.InvariantCulture))),
			    FuelStationCount = int.Parse(secondLine[3])
		    };

		    return result;
	    }

	    public void SaveToFile((double, List<Node>) data, string path)
	    {
		    var result = data.Item1.ToString("F") + Environment.NewLine +
		                 string.Join(" ", data.Item2.Select(x => x.Number.ToString()));
		    File.WriteAllText(path, result);
	    }

	    public (double, List<Node>) Solve(Task6Info inputData)
	    {
		    var nodes = new List<Node>();
		    for (var i = 0; i < inputData.FuelStationCount + 2; i++) nodes.Add(new Node(i));
		    var stations = new List<(int LengthFromStart, double FuelCost)>();
		    stations.Add((0, 0));
		    stations.AddRange(inputData.FuelStations);
		    stations.Add((inputData.AllPathLength, 0));
		    

		    for (var i = 0; i < stations.Count - 1; i++)
		    {
			    (Node, double) lastHopeEdge = (null, -1d);
			    for (var j = i + 1; j < stations.Count; j++)
			    {
				    var lengthBetween = stations[j].LengthFromStart - stations[i].LengthFromStart;
				    var canArriveTo = inputData.FuelPerLength * inputData.FuelTankVolume >= lengthBetween;
				    if (!canArriveTo) continue;
				    var needArriveTo = 2 * (double) lengthBetween / inputData.FuelPerLength > inputData.FuelTankVolume;

				    var cost = Math.Round((double) lengthBetween / inputData.FuelPerLength * stations[j].FuelCost, 2) + (i == 0 ? 0 : 20);
				    if (needArriveTo) nodes[i].To.Add((nodes[j], cost));
				    else lastHopeEdge = (nodes[j], cost);
			    }
			    if (!nodes[i].To.Any() && lastHopeEdge.Item1 != null) nodes[i].To.Add(lastHopeEdge);
		    }

		    var path = Dijkstra(nodes.First(), nodes.First(), nodes.Last());
		    return (path.Item1 + inputData.StartSpentMoney, path.Item2.Skip(1).Take(path.Item2.Count - 2).ToList());
	    }

	    public void Solve(string inputFilePath, string outputFilePath)
	    {
		    SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
	    }
    }
}