using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab8
{
    internal class Passenger
    {
        private static int passengersWithLuggageCount;
        private int id;
        private string name;

        public static int PassengersWithLuggageCount
        {
            get { return passengersWithLuggageCount; }
            set { if (value >= 0) passengersWithLuggageCount = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Passenger(string name, int id, bool hasLuggage)
        {
            this.name = name;
            this.id = id;

            if (hasLuggage)
            {
                passengersWithLuggageCount++;
            }
        }
    }
}
