using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LAB_1_MAUI.Lab_3.Entities;

namespace LAB_1_MAUI.Lab_3.Services
{
    public interface IRateService
    {
        Task<IEnumerable<Rate>> GetRates(DateTime date);
    }
}
