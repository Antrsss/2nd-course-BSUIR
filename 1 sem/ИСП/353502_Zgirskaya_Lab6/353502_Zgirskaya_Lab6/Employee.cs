using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab6
{
    internal class Employee
    {
        private string _name;
        private int _age;
        private bool _workDone;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Age 
        {
            get { return _age; }
            set { _age = value; } 
        }
        public bool WorkDone 
        {
            get { return _workDone; } 
            set { _workDone = value; } 
        }

        public Employee(string name, int age, bool workDone) 
        { 
            Name = name;
            Age = age;
            WorkDone = workDone;
        }
    }
}
