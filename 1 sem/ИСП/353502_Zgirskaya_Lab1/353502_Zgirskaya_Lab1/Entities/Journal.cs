using _353502_Zgirskaya_Lab1.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab1.Entities
{
    internal class Journal
    {
        private MyCustomCollection<string> _events;
        public Journal() 
        { 
            _events = new MyCustomCollection<string>();
        }
        public void LogEvent(object entity, string info) 
        {
            string eventInfo = $"Event: {info}. Entity: {entity.ToString}";
            _events.Add(eventInfo);
        }
        public void WriteAllEvents() 
        {
            Console.WriteLine("Journal Events:");
            foreach (var eventItem in _events)
            {
                Console.WriteLine(eventItem);
            }
        }
    }
}
