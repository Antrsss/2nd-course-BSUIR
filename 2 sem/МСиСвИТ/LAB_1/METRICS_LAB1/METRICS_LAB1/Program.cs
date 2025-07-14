using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace METRICS_LAB1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = File.ReadAllText("C:\\Users\\zgdas\\2 курс\\2 сем\\МСиСвИТ\\LAB_1\\example.txt");

            var metrics = new HalsteadMetrics();
            metrics.CalculateMetrics(code);
            metrics.PrintMetrics();
        }
    }
}
