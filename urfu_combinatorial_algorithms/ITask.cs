namespace urfu_combinatorial_algorithms
{
    public interface ITask<TSolveResult, TSolveInputData>
    {
        TSolveInputData LoadFromFile(string filePath);
        void SaveToFile(TSolveResult data, string path);
        TSolveResult Solve(TSolveInputData inputData);
    }
}