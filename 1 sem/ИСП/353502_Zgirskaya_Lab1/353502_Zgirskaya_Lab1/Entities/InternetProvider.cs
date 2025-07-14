using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace _353502_Zgirskaya_Lab1.Entities
{
    internal class InternetProvider<T> : Contracts.IInternetOperator<T> where T : INumber<T>
    {
        public event Action<object, string> TariffListChanged;
        public event Action<object, string> ClientListChanged;
        public event Action<object, string> ClientUsedTraffic;

        private Collections.MyCustomCollection<Client<T>> _clients;
        private Collections.MyCustomCollection<Tariff<T>> _tariffs;

        public InternetProvider()
        {
            _clients = new Collections.MyCustomCollection<Client<T>>();
            _tariffs = new Collections.MyCustomCollection<Tariff<T>>();
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

        public string FindFavoriteClient()
        {
            T maxClientPayout = T.CreateChecked(0);
            T currentClientPayout = T.CreateChecked(0);
            string favoriteClientName = "";

            for (int i = 0; i < _clients.Count; ++i)
            {
                for (int j = 0; j < _clients[i].ClientInternetInfo.Count; ++j)
                {
                    currentClientPayout += _clients[i].ClientInternetInfo[j].Tariff.Cost * _clients[i].ClientInternetInfo[j].Traffic;
                }
                if (currentClientPayout > maxClientPayout)
                {
                    maxClientPayout = currentClientPayout;
                    favoriteClientName = _clients[i].GetClientName();
                }
                currentClientPayout = T.CreateChecked(0);
            }

            return favoriteClientName;
        }

        public void RegisterClient(string user, Tariff<T>[] tariffs)
        {
            Client<T> newClient = new Client<T>(user);
            for (int i = 0; i < tariffs.Length; ++i)
            {
                newClient.ClientInternetInfo.Add(new InternetInfo<T>(tariffs[i]));
            }
            _clients.Add(newClient);

            ClientListChanged.Invoke(this, $"Client { newClient.ClientName } was added");
        }

        public void SetTariffs(Tariff<T>[] tariffs)
        {
            for (int i = 0; i < tariffs.Length; ++i)
            {
                _tariffs.Add(tariffs[i]);

                TariffListChanged?.Invoke(this, $"Tariff { tariffs[i].Name } was added");
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
    }
}
