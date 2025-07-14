using _353502_Zgirskaya_Lab3.Entities;

namespace _353502_Zgirskaya_Lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InternetOperator<int> internetOperator = new InternetOperator<int>();

            Journal journal = new Journal();
            internetOperator.TariffListChanged += journal.LogEvent;
            internetOperator.ClientListChanged += journal.LogEvent;


            Action<object, string> EventHandler = (object entity, string info) =>
            {
                Console.WriteLine(info);
            };
            internetOperator.ClientUsedTraffic += EventHandler;


            Tariff<int> tariff1 = new Tariff<int>("A1", 5); //2
            Tariff<int> tariff2 = new Tariff<int>("MTC", 10); //1
            Tariff<int> tariff3 = new Tariff<int>("Life", 15); //2
            internetOperator.SetTariffs(new[] { tariff1, tariff2, tariff3 });

            internetOperator.RegisterClient("Tom", new[] { tariff1, tariff2 });
            internetOperator.RegisterClient("Jerry", new[] { tariff3 });
            internetOperator.RegisterClient("Matt", new[] { tariff1, tariff3 });
            journal.WriteAllEvents();

            internetOperator.SetClientUsedTraffic("Tom", new[] { 10, 100, 1000 });
            internetOperator.SetClientUsedTraffic("Jerry", new[] { 10 });
            internetOperator.SetClientUsedTraffic("Matt", new[] { 10, 500});

            Console.WriteLine();
            var allTariffNames = internetOperator.GetAllTariffNames();
            Console.WriteLine();
            var allTrafficCost = internetOperator.GetCommonTrafficCost();
            Console.WriteLine();
            var favoriteClientName = internetOperator.GetFavoriteClientName();
            Console.WriteLine();
            var specialCostClients = internetOperator.GetSpecialCostClients();
            Console.WriteLine();
            var tariffsAllClients = internetOperator.GetTariffAllClientsNumber();
        }
    }
}
