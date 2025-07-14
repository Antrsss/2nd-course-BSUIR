using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zgirskaya_353502_Lab5.Domain.Entities;

namespace Zgirskaya_353502_Lab5.Domain.Interfaces
{
    public interface ISerializer
    {
        IEnumerable<Actor> DeSerializeByLINQ(string fileName);
        IEnumerable<Actor> DeSerializeXML(string fileName);
        IEnumerable<Actor> DeSerializeJSON(string fileName);
        void SerializeByLINQ(IEnumerable<Actor> xxx, string fileName);
        void SerializeXML(IEnumerable<Actor> xxx, string fileName);
        void SerializeJSON(IEnumerable<Actor> xxx, string fileName);

    }
}
