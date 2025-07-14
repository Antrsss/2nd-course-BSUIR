namespace JilMetrics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = File.ReadAllText("C:\\Users\\zgdas\\2 курс\\2 сем\\МСиСвИТ\\LAB_2\\test.txt");

            var metrics = new JilbsMetrics();
            metrics.CalculateMetrics(code);
            metrics.CalculateBranchingAndDepth();
            metrics.PrintMetrics();
        }
    }
}
