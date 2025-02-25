using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Evntaly
{
    public class SDK
    {
        private readonly string _developerSecret;
        private readonly string _projectToken;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://evntaly.com/prod";

        private bool _trackingEnabled = true;

        public SDK(string developerSecret, string projectToken)
        {
            _developerSecret = developerSecret ?? throw new ArgumentNullException(nameof(developerSecret));
            _projectToken = projectToken ?? throw new ArgumentNullException(nameof(projectToken));

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("pat", _projectToken);
            _httpClient.DefaultRequestHeaders.Add("secret", _developerSecret);
        }

        public async Task<bool> CheckLimitAsync()
        {
            string url = $"{_baseUrl}/api/v1/account/check-limits/{_developerSecret}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Error checking limit: {response.ReasonPhrase}");
                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(responseContent);

            return json.GetProperty("limitReached").GetBoolean() == false;
        }

        public async Task<bool> TrackEventAsync(Event eventData)
        {
            if (!_trackingEnabled)
            {
                Console.WriteLine("🚫 Tracking is disabled. Event not sent.");
                return false;
            }

            if (!await CheckLimitAsync())
            {
                Console.WriteLine("❌ Tracking limit reached. Event not sent.");
                return false;
            }

            string url = $"{_baseUrl}/api/v1/register/event";
            var jsonContent = new StringContent(JsonSerializer.Serialize(eventData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ Event tracked successfully!");
                return true;
            }
            else
            {
                Console.WriteLine($"❌ Error tracking event: {response.ReasonPhrase}");
                return false;
            }
        }

        public async Task<bool> IdentifyUserAsync(UserProfile userData)
        {
            string url = $"{_baseUrl}/api/v1/register/user";
            var jsonContent = new StringContent(JsonSerializer.Serialize(userData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ User identified successfully!");
                return true;
            }
            else
            {
                Console.WriteLine($"❌ Error identifying user: {response.ReasonPhrase}");
                return false;
            }
        }

        public void DisableTracking()
        {
            _trackingEnabled = false;
            Console.WriteLine("🚫 Tracking disabled.");
        }

        public void EnableTracking()
        {
            _trackingEnabled = true;
            Console.WriteLine("🟢 Tracking enabled.");
        }
    }

    public class Event
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }

        [JsonPropertyName("notify")]
        public bool Notify { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("apply_rule_only")]
        public bool ApplyRuleOnly { get; set; }

        [JsonPropertyName("user")]
        public EventUser User { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("sessionID")]
        public string SessionID { get; set; }

        [JsonPropertyName("feature")]
        public string Feature { get; set; }

        [JsonPropertyName("topic")]
        public string Topic { get; set; }
    }

    public class EventUser
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
    }

    public class UserProfile
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("organization")]
        public string Organization { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}
