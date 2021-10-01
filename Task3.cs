using CombinatorialAlgorithms;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CombinatorialAlgorithms
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

        public (Info, Dictionary<int, int>) Sort(Info inputData)
        {
            // НАЧАЛО ТОПОЛОГИЧЕСКОЙ СОРТИРОВКИ
            var newGraph = new List<List<(int, int)>>();
            var savedGraph = new List<List<(int, int)>>();
            for (int i = 0; i < inputData.Next.Count(); i++)
            {
                var l = new List<(int, int)>();
                for (int j = 0; j < inputData.Next[i].Count; j++)
                {
                    l.Add(inputData.Next[i][j]);
                }
                savedGraph.Add(l);
            }

            var translation = new Dictionary<int, int>();
            var maxUsefullNum = inputData.Next.Count() - 1;


            

            while (maxUsefullNum >= 0)
            {
                var cur = -1;
                for (var i = 0; i < savedGraph.Count(); i++)
                    if (savedGraph[i] != null && savedGraph[i].Count == 0)
                    {
                        translation[cur = i] = maxUsefullNum--;
                        break;
                    }

                savedGraph[cur] = null;
                for (var i = 0; i < savedGraph.Count; i++)
                {
                    if (savedGraph[i] is null) continue;
                    savedGraph[i] = savedGraph[i].Where(x => x.Item1 != cur).ToList();
                }
            }



            var result = new Info();
            result.Start = translation[inputData.Start];
            result.Target = translation[inputData.Target];
            var a = new List<(int, int)>[translation.Count];
            for (int i = 0; i < translation.Count; i++)
            {
                a[translation[i]] = inputData.Next[i];
                for (int j = 0; j < a[translation[i]].Count; j++)
                {
                    a[translation[i]][j] = (translation[a[translation[i]][j].Item1], a[translation[i]][j].Item2);
                }
            }
            result.Next = a.ToList();
            // КОНЕЦ ТОПОЛОГИЧЕСКОЙ СОРТИРОВКИ

            var reverseTranslation = new Dictionary<int, int>();
            foreach (var key in translation)
            {
                reverseTranslation[key.Value] = key.Key;
            }
            return (result, reverseTranslation);
        }

        public List<string> Solve(Info inputData)
        {
            var (d, t) = Sort(inputData);
            inputData = d;
            var D = new int[inputData.Next.Count];
            var previous = new int[D.Length];
            var result1 = new List<string> { "N" };


            for (var i = 0; i < D.Length; i++) D[i] = int.MaxValue;
            D[inputData.Start] = 0;

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
                            result.Add(t[n].ToString());
                            n = previous[n];
                        }

                        result.Reverse();
                        result1 = new List<string> { "Y", string.Join(" ", result), D[v].ToString() };
                    }
                }
            }

            return result1;
        }

        public void Solve(string inputFilePath, string outputFilePath)
        {
            SaveToFile(Solve(LoadFromFile(inputFilePath)), outputFilePath);
        }
    }
}
