using CombinatorialAlgorithms;
using FluentApi.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace urfu_combinatorial_algorithms
{
    public class Task3 : ITask<List<string>, Graph>, ITask
    {
        public Graph LoadFromFile(string filePath)
        {
            var data = File.ReadAllLines(filePath);
            var graphInfo = data.Skip(1).Take(int.Parse(data[0])).ToArray();

            var gb = DotGraphBuilder.DirectedGraph("");
            for (var i = 0; i < graphInfo.Count(); i++)
            {
                var line = graphInfo[i].Split(' ');
                for (var j = 0; j < line.Length && line[j] != "0"; j+=2)
                {
                    gb = gb.AddEdge((i + 1).ToString(), line[j]).With(c => c.Weight(double.Parse(line[j + 1])));
                }
            }

            return gb.Build();
        }

        public void SaveToFile(List<string> data, string path)
        {
            File.WriteAllLines(path, data);
        }

        public void Solve(string inputFilePath, string outputFilePath)
        {
            SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
        }

        public List<string> Solve(Graph inputData)
        {
            throw new NotImplementedException();
        }
    }
}
