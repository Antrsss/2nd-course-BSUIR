using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab4.Entities
{
    internal class MyCustomComparer : IComparer<PassengerLuggage>
    {
        public int Compare(PassengerLuggage? x, PassengerLuggage? y)
        {
            if (x.Name == y.Name)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
