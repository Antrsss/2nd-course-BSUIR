using System.Diagnostics;

namespace _353502_Zgirskaya_Lab7
{
    public class IntegralCalculator
    {
        public event Action<double, long> CalculationCompleted;
        public event Action<double> ProgressUpdated;

        private static readonly object singleThreadLock = new object();
        private static Semaphore semaphore;

        public IntegralCalculator(int maxThreads)
        {
            semaphore = new Semaphore(maxThreads, maxThreads,"TestSemaphore");
        }
        public IntegralCalculator() { }

        public double CalculateIntegralSingleThread()
        {
            lock (singleThreadLock)
            {
                return CalculateIntegral();
            }
        }

        public double CalculateIntegralLimitedThreads()
        {
            semaphore.WaitOne();

            try
            {
                return CalculateIntegral();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public double CalculateIntegral()
        {
            double a = 0;
            double b = 1;
            double step = 0.00000001;
            //double step = 0.1;
            double result = 0;
            int delayOperations = 100000;
            //int delayOperations = 1;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (double x = a; x < b; x += step)
            {
                result += Math.Sin(x) * step;

                for (int i = 1; i <= delayOperations; i++)
                {
                    double temp = 1.0 * 2.0;
                }

                double progress = (x - a) / (b - a) * 100;
                ProgressUpdated?.Invoke(progress);
            }

            stopwatch.Stop();

            CalculationCompleted?.Invoke(result, stopwatch.ElapsedTicks);
            
            return result;
        }
    }
}
