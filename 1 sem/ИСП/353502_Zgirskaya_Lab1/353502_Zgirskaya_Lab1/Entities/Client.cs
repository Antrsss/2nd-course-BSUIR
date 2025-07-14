using _353502_Zgirskaya_Lab1.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab1.Entities
{
    internal class Client<T> where T : INumber<T>
    {
        private string _clientName;

        public MyCustomCollection<InternetInfo<T>> ClientInternetInfo;
        public Client(string name)
        {
            ClientInternetInfo = new MyCustomCollection<InternetInfo<T>>();
            _clientName = name;
        }
        public string GetClientName()
        {
            return _clientName;
        }

        public string ClientName
        {
            get { return _clientName; }
        }
    }
}
