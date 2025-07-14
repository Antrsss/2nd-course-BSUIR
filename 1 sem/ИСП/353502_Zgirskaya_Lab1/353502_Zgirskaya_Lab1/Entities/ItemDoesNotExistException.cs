using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace _353502_Zgirskaya_Lab1.Entities
{
    internal class ItemDoesNotExistException : Exception
    {
        public override string Message => "Exception: Item does not exist in this collection!";
    }
}
