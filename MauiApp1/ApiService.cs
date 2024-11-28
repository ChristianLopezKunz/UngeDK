using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MauiApp1
{
    public class JobDetail
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string Resume { get; set; }
        public string Content { get; set; }
        public string ModifiedDate { get; set; }
        public string ApplicationDate { get; set; }
        public string ApplicationUrl { get; set; }
    }

    public class JobDetailApiResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public JobDetail Data { get; set; }
    }

    public class ApiService
    {
        private const string ApiKey = "7F125C9A-36FB-41BB-BBFB-73813D7BE40D"; // Din hårdkodede API-nøgle
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<JobDetailApiResponse> GetJobDetailAsync(int jobId)
        {
            try
            {
                string url = $"https://studerendeonline.dk/api/ungdk/job/{jobId}?apikey={ApiKey}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialiser JSON-svaret til vores model
                var result = JsonConvert.DeserializeObject<JobDetailApiResponse>(jsonResponse);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl: {ex.Message}");
                return null;
            }
        }
    }


}
