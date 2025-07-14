using _353502_Zgirskaya_Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab3.Entities
{
    internal class Journal
    {
        private List<string> _events;
        public Journal() 
        { 
            _events = new List<string>();
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
