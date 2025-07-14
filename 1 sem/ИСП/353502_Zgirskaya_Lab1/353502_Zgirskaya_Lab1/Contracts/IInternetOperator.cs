using _353502_Zgirskaya_Lab1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab1.Contracts
{
    internal interface IInternetOperator<T> where T : INumber<T>
    {
        void SetTariffs(Tariff<T>[] tariffs);

        void RegisterClient(string user, Tariff<T>[] tariffs);

        void SetClientUsedTraffic(string user, T[] traffic);

        T CalculateAllUsedTrafficPayout();

        string FindFavoriteClient();

        public T CalculateOneTariffPayout(string tariffName);
    }
}
