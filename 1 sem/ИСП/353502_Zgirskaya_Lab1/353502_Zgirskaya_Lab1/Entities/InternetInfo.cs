using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab1.Entities
{
    internal class InternetInfo<T> where T : INumber<T>
    {
        private Tariff<T> _tariff;
        private T _traffic;

        public InternetInfo(Tariff<T> tariff)
        {
            _tariff = tariff;
            _traffic = T.CreateChecked(0);
        }

        public T Traffic 
        { 
            get { return _traffic; }
            set { _traffic = value; }
        }
        public Tariff<T> Tariff 
        {
            get { return _tariff; } 
            set { _tariff = value; } 
        }
    }
}
