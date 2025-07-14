using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab3.Entities
{
    internal class Tariff<T> where T : INumber<T>
    {
        public Tariff(string name, T cost)
        {
            _name = name;
            _cost = cost;
        }
        private string _name;
        private T _cost;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public T Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }
    }
}
