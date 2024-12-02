using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MauiApp1
{
    public class ApiService
    {
        private const string ApiKey = "7F125C9A-36FB-41BB-BBFB-73813D7BE40D";
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        // Fetch job list from API
        public async Task<JobListApiResponse> GetJobsAsync(string keyword = "", int max = 50, int page = 1, List<int> areas = null)
        {
            try
            {
                string areaParam = areas != null ? string.Join(",", areas) : string.Empty;
                string url = $"https://studerendeonline.dk/api/ungdk/jobs?apikey={ApiKey}&key={Uri.EscapeDataString(keyword)}&max={max}&page={page}&amt={areaParam}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JobListApiResponse>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }

    // API Response model
    public class JobListApiResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public int Total { get; set; }
        public List<Job> Jobs { get; set; }
    }

    // Job model
    public class Job
    {
        public string Id { get; set; }
        public double Score { get; set; }
        public string ModifiedDate { get; set; }
        public string ApplicationDate { get; set; }
        public string JobTitle { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Resume { get; set; }
        public int JobLevel { get; set; }
        public List<int> Geography { get; set; }
    }
}
