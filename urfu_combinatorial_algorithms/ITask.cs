namespace CombinatorialAlgorithms
{
    public interface ITask<TSolveResult, TSolveInputData>
    {
        TSolveInputData LoadFromFile(string filePath);
        void SaveToFile(TSolveResult data, string path);
        TSolveResult Solve(TSolveInputData inputData);
    }
    
    public interface ITask
    {
        void Solve(string inputFilePath, string outputFilePath);
    }
}