using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public async Task<JobDetailsResponse> GetJobDetailsAsync(string jobId)
        {
            try
            {
                string url = $"https://studerendeonline.dk/api/ungdk/job/{jobId}?apikey={ApiKey}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JobDetailsResponse>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

    }
    // Job details response model
    public class JobDetailsResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Job Data { get; set; }
    }
    // Job list API Response model
    public class JobListApiResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public int Total { get; set; }
        public List<Job> Jobs { get; set; }
    }
    public class JobDetails
    {
        public string CompanyName { get; set; }
        public string ApplicationDeadline { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
        public string ApplicationUrl { get; set; }
    }

    // Job list model
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
        public string ApplicationUrl { get; set; }
        public string CompanyAndGeography => $"{CompanyName}, {GeographyDisplay}";



        // Static mapping of geography IDs to names
        private static readonly Dictionary<int, string> GeographyMapping = new Dictionary<int, string>
    {
        { 2, "Storkøbenhavn" },
        { 3, "Nordsjælland" },
        { 14, "Østsjælland" },
        { 4, "Vestsjælland" },
        { 5, "Sydsjælland & Øer" },
        { 13, "Fyn" },
        { 12, "Sønderjylland" },
        { 11, "Sydvestjylland (Esbjerg)" },
        { 9, "Vestjylland" },
        { 10, "Sydøstjylland" },
        { 7, "Midtjylland" },
        { 6, "Østjylland (Aarhus)" },
        { 8, "Nordjylland" },
        { 20, "Bornholm" }
    };

        public string GeographyDisplay
        {
            get
            {
                if (Geography == null || !Geography.Any())
                    return "Ingen resultater";

                var geographyNames = Geography
                    .Where(id => GeographyMapping.ContainsKey(id))
                    .Select(id => GeographyMapping[id]);

                return geographyNames.Any() ? string.Join(", ", geographyNames) : "Ingen resultater";
            }
        }

        public string ApplicationDeadlineDisplay
        {
            get
            {
                return string.IsNullOrWhiteSpace(ApplicationDate) ? "snarest muligt" : ApplicationDate;
            }
        }

    }
}
