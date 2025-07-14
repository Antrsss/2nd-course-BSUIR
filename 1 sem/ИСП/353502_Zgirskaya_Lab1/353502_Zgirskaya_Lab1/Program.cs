using _353502_Zgirskaya_Lab1.Collections;
using _353502_Zgirskaya_Lab1.Entities;

namespace _353502_Zgirskaya_Lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InternetProvider<int> internetProvider = new InternetProvider<int>();

            Journal journal = new Journal();
            internetProvider.TariffListChanged += journal.LogEvent;
            //internetProvider.ClientListChanged += journal.LogEvent;


            Action<object, string> EventHandler = (object entity, string info) =>
            {
                Console.WriteLine(info);
            };
            internetProvider.ClientUsedTraffic += EventHandler;


            Tariff<int> tariff1 = new Tariff<int>("A1", 5);
            Tariff<int> tariff2 = new Tariff<int>("MTC", 10);
            Tariff<int> tariff3 = new Tariff<int>("Life", 15);
            internetProvider.SetTariffs(new[] { tariff1, tariff2, tariff3 });

            internetProvider.RegisterClient("Tom", new[] { tariff1, tariff2 });
            internetProvider.RegisterClient("Jerry", new[] { tariff3 });
            internetProvider.RegisterClient("Tom", new[] { tariff1, tariff2, tariff3 });
            journal.WriteAllEvents();

            internetProvider.SetClientUsedTraffic("Tom", new[] { 10, 100, 1000 });
            internetProvider.SetClientUsedTraffic("Jerry", new[] { 10 });

            int allPayout = internetProvider.CalculateAllUsedTrafficPayout(); //17'250
            int firstTariffPayout = internetProvider.CalculateOneTariffPayout("A1"); //100
            string favoriteClient = internetProvider.FindFavoriteClient();

            Console.WriteLine("All payout: " + allPayout.ToString());
            Console.WriteLine("First trafffic payout: " + firstTariffPayout.ToString());
            Console.WriteLine("Favorite client: " + favoriteClient.ToString());
            Console.WriteLine();


            //Collection
            MyCustomCollection<double> myCustomCollection = new MyCustomCollection<double>();
            myCustomCollection.Add(1);
            myCustomCollection.Add(2);
            myCustomCollection.Add(3);
            myCustomCollection.Add(4);

            // 1 2 3 4
            for (int i = 0; i < myCustomCollection.Count; i++)
            {
                Console.WriteLine(myCustomCollection[i].ToString());
            }
            Console.WriteLine();

            //////////////////////////////////////////

            try
            {
                myCustomCollection.Remove(5);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
            /*for (int i = 0; i < myCustomCollection.Count; i++)
            {
                Console.WriteLine(myCustomCollection[i].ToString());
            }
            Console.WriteLine();*/

            try
            {
                Console.WriteLine(myCustomCollection[5]);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
            

            //////////////////////////////////////////

            myCustomCollection.Reset();
            myCustomCollection.RemoveCurrent();
            // 3 4
            for (int i = 0; i < myCustomCollection.Count; i++)
            {
                Console.WriteLine(myCustomCollection[i].ToString());
            }
            Console.WriteLine();

            myCustomCollection.Next();
            myCustomCollection.RemoveCurrent();
            // 4
            for (int i = 0; i < myCustomCollection.Count; i++)
            {
                Console.WriteLine(myCustomCollection[i].ToString());
            }
            Console.WriteLine();

        }
    }
}
