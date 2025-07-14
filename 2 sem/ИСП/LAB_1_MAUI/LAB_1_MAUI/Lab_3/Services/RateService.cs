using LAB_1_MAUI.Lab_3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LAB_1_MAUI.Lab_3.Services
{
    public class RateService : IRateService
    {
        private readonly HttpClient _httpClient;

        private IEnumerable<Rate> Items { get; set; }

        public RateService(HttpClient httpClientFactory)
        {
            _httpClient = httpClientFactory;//.CreateClient("RB Currencies");
        }
        public async Task<IEnumerable<Rate>> GetRates(DateTime date)
        {
            try
            {
                string dateString = date.ToString("yyyy-MM-dd");
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<Rate>>($"?ondate={dateString}&periodicity=0");

                if (response != null)
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching rates: {ex.Message}");
            }

            return Items;
        }
    }
}
