using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab4.Entities
{
    internal class PassengerLuggage
    {
        private string _name;
        private int _weight;
        private bool _isSafe;

        public PassengerLuggage(string name, int weight, bool isSafe) 
        { 
            _name = name;
            _weight = weight;
            _isSafe = isSafe;
        }
        public string Name 
        { 
            get { return _name; } 
            set { _name = value; } 
        }
        public int Weight 
        {   
            get { return _weight; }
            set { _weight = value; } 
        }
        public bool IsSafe 
        {
            get { return _isSafe; }
            set { _isSafe = value; }
        }

        public override string ToString()
        {
            return $"Lagguage: name = {_name}, weight = {_weight}, safety = {_isSafe}";
        }
    }
}
