using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using _353502_Zgirskaya_Lab3.Contracts;


namespace _353502_Zgirskaya_Lab3.Entities
{
    internal class InternetOperator<T> : IInternetOperator<T> where T : INumber<T>
    {
        public event Action<object, string> TariffListChanged;
        public event Action<object, string> ClientListChanged;
        public event Action<object, string> ClientUsedTraffic;

        private int _specialCost = 1000;

        private List<Client<T>> _clients;
        private Dictionary<string, T> _tariffs;

        public InternetOperator()
        {
            _clients = new List<Client<T>>();
            _tariffs = new Dictionary<string, T>();
        }

        public T CalculateAllUsedTrafficPayout() 
        {
            T payout = T.CreateChecked(0);

            for (int i = 0; i < _clients.Count; ++i)
            {
                for (int j = 0; j < _clients[i].ClientInternetInfo.Count; ++j)
                {
                    payout += _clients[i].ClientInternetInfo[j].Tariff.Cost * _clients[i].ClientInternetInfo[j].Traffic;
                }
            }

            return payout;
        }

        public T CalculateOneTariffPayout(string tariffName)
        {
            T payout = T.CreateChecked(0);
            for (int i = 0; i < _clients.Count; ++i)
            {
                for (int j = 0; j < _clients[i].ClientInternetInfo.Count; ++j)
                {
                    if (_clients[i].ClientInternetInfo[j].Tariff.Name == tariffName)
                    {
                        payout += _clients[i].ClientInternetInfo[j].Traffic * _clients[i].ClientInternetInfo[j].Tariff.Cost;
                    }
                }
            }

            return payout;
        }

        public void RegisterClient(string user, Tariff<T>[] tariffs)
        {
            Client<T> newClient = new Client<T>(user);
            for (int i = 0; i < tariffs.Length; ++i)
            {
                newClient.ClientInternetInfo.Add(new InternetInfo<T>(tariffs[i]));
            }
            _clients.Add(newClient);

            ClientListChanged?.Invoke(this, $"Client { newClient.ClientName } was added");
        }

        public void SetTariffs(Tariff<T>[] tariffs)
        {
            for (int i = 0; i < tariffs.Length; ++i)
            {
                _tariffs.Add(tariffs[i].Name, tariffs[i].Cost);

                TariffListChanged?.Invoke(this, $"Tariff { tariffs[i].Name } was added with clientInternetTariff {tariffs[i].Cost }");
            }
        }

        public void SetClientUsedTraffic(string user, T[] traffic)
        {
            for (int i = 0; i < _clients.Count; ++i)
            {
                if (_clients[i].GetClientName().Equals(user))
                {
                    for (int j = 0; j < _clients[i].ClientInternetInfo.Count; ++j)
                    {
                        _clients[i].ClientInternetInfo[j].Traffic = traffic[j];

                        ClientUsedTraffic?.Invoke(this, $"Client { _clients[i].ClientName } has used { traffic[j] } traffic");
                    }
                }
            }
        }

        //1---
        public List<string> GetAllTariffNames()
        {
            var descCostTariffDict = (from t in _tariffs 
                                     orderby t.Value descending 
                                     select t.Key).ToList();

            //List<string> descCostTariffList = new List<string>();
            //Console.WriteLine("Ordered cost descending list:");

            //foreach (var item in descCostTariffDict)
            //{
            //    descCostTariffList.Add(item.Key);
            //    Console.WriteLine($"Tariff {item.Key} with cost {item.Value}");
            //}

            return descCostTariffDict;
        }

        //2---
        public T GetCommonTrafficCost()
        {
            T commonTrafficCost = _clients
                .Aggregate(
                    T.Zero, (commonCost, client) =>
                    commonCost + client.ClientInternetInfo
                    .Aggregate(
                        T.Zero, (clientCost, internetTariffInfo) =>
                        clientCost + (internetTariffInfo.Traffic * internetTariffInfo.Tariff.Cost)
                    )
                );

            Console.WriteLine("Common traffic cost: " + commonTrafficCost.ToString());

            return commonTrafficCost;
        }

        //3---
        public string GetFavoriteClientName()
        {
            Dictionary<string, T> allClientsPayout = new Dictionary<string, T>();
            T currentPayout = T.CreateChecked(0);

            for (int i = 0; i < _clients.Count; ++i)
            {
                currentPayout = _clients[i].ClientInternetInfo
                    .Aggregate(
                            T.Zero, (clientPayout, clientInternetInfo) =>
                            clientPayout + clientInternetInfo.Traffic * clientInternetInfo.Tariff.Cost
                    );

                allClientsPayout.Add(_clients[i].ClientName, currentPayout);
            }

            /*for (int i = 0; i < _clients.Count; ++i)
            {
                for (int j = 0; j < _clients[i].ClientInternetInfo.Count; ++j)
                {
                    currentPayout += _clients[i].ClientInternetInfo[j].Tariff.Cost * _clients[i].ClientInternetInfo[j].Traffic;
                }
                allClientsPayout.Add(_clients[i].ClientName, currentPayout);
                currentPayout = T.CreateChecked(0);
            }*/

            var maxPayout = (from c in allClientsPayout select c).Max(c => c.Value);
            Console.WriteLine("Max client payout: " + maxPayout.ToString());

            var favoriteClient = allClientsPayout.Where(c => c.Value == maxPayout).First();
            Console.WriteLine("Client name: " + favoriteClient.Key);

            return favoriteClient.Key;
        }

        //4---
        public int GetSpecialCostClients()
        {
            int specialCostClients = _clients
                .Aggregate(
                    0, (clientNumber, client) =>
                    client.ClientInternetInfo
                    .Aggregate(
                        T.Zero, (clientPayout, clientInternetTariff) => 
                        clientPayout + (clientInternetTariff.Traffic * clientInternetTariff.Tariff.Cost)
                    ) > T.CreateChecked(_specialCost)
                ? clientNumber + 1
                : clientNumber
                );

            Console.WriteLine("Number of special cost clients: " + specialCostClients.ToString());

            return specialCostClients;
        }

        //5---
        public Dictionary<string, int> GetTariffAllClientsNumber()
        {
            var tariffByClientsNumber = _clients
                .SelectMany(client => client.ClientInternetInfo)
                .GroupBy(info => info.Tariff.Name)
                .ToDictionary(group => group.Key, group => group.Count());

            foreach (var tariff in tariffByClientsNumber) 
            {
                Console.WriteLine($"Tariff {tariff.Key} has {tariff.Value} clients");
            }

            return tariffByClientsNumber;
        }
    }
}
