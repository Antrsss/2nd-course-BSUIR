using LoremNET;

namespace _353502_Zgirskaya_Lab8
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Start");

            var passengers = new List<Passenger>();
            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                string name = Lorem.Words(1);
                int id = i + 1;
                bool hasLuggage = random.Next(2) == 1;

                passengers.Add(new Passenger(name, id, hasLuggage));
            }

            Console.WriteLine($"Count of passengers with luggage: {Passenger.PassengersWithLuggageCount}");

            var progress = new Progress<string>(message => Console.WriteLine(message));
            var service = new StreamService<Passenger>();

            using (var memoryStream = new MemoryStream())
            {
                Task writeTask = service.WriteToStreamAsync(memoryStream, passengers, progress);
                Task.Delay(100);
                Task copyTask = service.CopyFromStreamAsync(memoryStream, "passengers_output.txt", progress);

                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Streams 1 and 2 started");

                await Task.WhenAll(writeTask, copyTask);
            }

            int statistics = await service.GetStatisticsAsync("passengers_output.txt", p => p.Name.Length > 5);

            Console.WriteLine($"Count passengers with name longer than 5 letters: {statistics}");
        }
    }
}