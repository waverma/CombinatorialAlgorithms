namespace urfu_combinatorial_algorithms
{
    public interface ITask<out TSolveResult, in TSolveInputData>
    {
        TSolveResult Solve(TSolveInputData inputData);
    }
}