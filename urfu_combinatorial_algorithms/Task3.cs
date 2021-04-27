using CombinatorialAlgorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urfu_combinatorial_algorithms
{
    public class Info
    {
        public List<List<(int, int)>> Next = new List<List<(int, int)>>();
        public int Start;
        public int Target;
    }

    public class Task3 : ITask<List<string>, Info>, ITask
    {
        public Info LoadFromFile(string filePath)
        {
            var data = File.ReadAllLines(filePath);
            var result = new Info();
            result.Target = int.Parse(data.Last()) - 1;

            foreach (var item in data.Skip(1).Take(int.Parse(data[0])).ToArray())
            {
                var line = item.Split(' ');
                var list = new List<(int, int)>();
                result.Next.Add(list);
                for (var j = 0; j < line.Length && line[j] != "0"; j += 2)
                {
                    list.Add((int.Parse(line[j]) - 1, int.Parse(line[j + 1])));
                }
            }

            return result;
        }

        public void SaveToFile(List<string> data, string path)
        {
            if (data.Count != 1)
                data[1] = string.Join(" ", data[1].Split(' ').Select(x => int.TryParse(x.ToString(), out var y) ? (y + 1).ToString() : x.ToString()));
            File.WriteAllLines(path, data);
        }

        public List<string> Solve(Info inputData)
        {
            var D = new int[inputData.Next.Count];
            var previous = new int[D.Length];

            for (var i = 1; i < D.Length; i++) D[i] = int.MaxValue;

            for (var i = 0; i < D.Length - 1; i++)
            {
                foreach (var (v, range) in inputData.Next[i])
                {
                    if (D[i] + range < D[v])
                    {
                        D[v] = D[i] + range;
                        previous[v] = i;
                    }

                    if (v == inputData.Target)
                    {
                        var result = new List<string>();
                        var n = inputData.Target;
                        while (n != 0)
                        {
                            result.Add(n.ToString());
                            n = previous[n];
                        }

                        result.Reverse();
                        return new List<string> { "Y", string.Join(" ", result), D[v].ToString() };
                    }
                }
            }

            return new List<string> { "N" };
        }

        public void Solve(string inputFilePath, string outputFilePath)
        {
            SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
        }
    }
}
