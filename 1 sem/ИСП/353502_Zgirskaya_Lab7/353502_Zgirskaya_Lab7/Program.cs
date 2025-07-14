using System.Reflection;

namespace _353502_Zgirskaya_Lab7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int maxNumThreads = 3;
            IntegralCalculator calculatorSingleThread = new IntegralCalculator();
            IntegralCalculator calculatorLimitedThreads = new IntegralCalculator(maxNumThreads);

            calculatorSingleThread.CalculationCompleted += OnCalculationCompleted;
            calculatorSingleThread.ProgressUpdated += OnProgressUpdated;

            calculatorLimitedThreads.CalculationCompleted += OnCalculationCompleted;
            calculatorLimitedThreads.ProgressUpdated += OnProgressUpdated;

            //Thread thread1 = new Thread(() => calculatorSingleThread.CalculateIntegral())
            //{
            //    Priority = ThreadPriority.Highest
            //};

            //Thread thread2 = new Thread(() => calculatorSingleThread.CalculateIntegral())
            //{
            //    Priority = ThreadPriority.Lowest
            //};

            //Console.WriteLine("Different priority threads:\n");
            //thread1.Start();
            //thread2.Start();

            //thread1.Join();
            //thread2.Join();

            //Console.WriteLine("\nSingle thread:\n");
            //Thread[] threadsSingle = new Thread[5];
            //for (int i = 0; i < threadsSingle.Length; i++)
            //{
            //    threadsSingle[i] = new Thread(() => calculatorSingleThread.CalculateIntegralSingleThread());
            //    threadsSingle[i].Start();
            //}

            //foreach (var thread in threadsSingle)
            //{
            //    thread.Join();
            //}

            Console.WriteLine($"\n{maxNumThreads} threads:\n");
            Thread[] threadsLimited = new Thread[5];
            for (int i = 0; i < threadsLimited.Length; i++)
            {
                threadsLimited[i] = new Thread(() => calculatorLimitedThreads.CalculateIntegralLimitedThreads());
                threadsLimited[i].Start();
            }

            foreach (var thread in threadsLimited)
            {
                thread.Join();
            }
        }

        static void OnCalculationCompleted(double result, long ticks)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Completed. Result: {result} for {ticks} ticks");
        }

        static void OnProgressUpdated(double progress)
        {
            switch((int)progress / 10)
            {
                case 0: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [] {progress}%");
                    break;
                case 1: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [=>] {progress}%");
                    break;
                case 2: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [==>] {progress}%");
                    break;
                case 3: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [===>] {progress}%");
                    break;
                case 4: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [====>] {progress}%");
                    break;
                case 5: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [=====>] {progress}%");
                    break;
                case 6: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [======>] {progress}%");
                    break;
                case 7: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [=======>] {progress}%");
                    break;
                case 8: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [========>] {progress}%");
                    break;
                case 9: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [=========>] {progress}%");
                    break;
                case 10: 
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: [==========] {progress}%");
                    break;
            }
        }
    }
}
