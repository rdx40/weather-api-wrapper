using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace WeatherApiWrapper.Services
{
      
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        //constructor to initialize the HttpClient
        //this is where we inject the HttpClient dependency
        //this allows us to use the HttpClient for making requests
        //the HttpClient is injected through the constructor   

        private readonly string _apiKey;
        public WeatherService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;

            // Try to read from environment variable first
            _apiKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY");

            // If not set in environment, fallback to appsettings.json
            if (string.IsNullOrEmpty(_apiKey))
            {
                _apiKey = config["WeatherApiKey"];
            }

            // Throw if no API key is found anywhere — fail fast
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("API key missing! Set WEATHER_API_KEY environment variable or add to appsettings.json");
            }
        }
        //async method to get weather data
        public async Task<WeatherResult?> GetWeatherAsync(string city)
        {
            //url for api request
            var url = $"http://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}&aqi=no";

            //sends get req
            var response = await _httpClient.GetAsync(url);
            //if repsonse not successful returns null
            if (!response.IsSuccessStatusCode)
                return null;
            //reads response as String
            var json = await response.Content.ReadAsStringAsync();
            //parses json in a jsondocument
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            //extracts data from the json document and returns a WeatherResult object
            return new WeatherResult
            {
                City = root.GetProperty("location").GetProperty("name").GetString(),
                Country = root.GetProperty("location").GetProperty("country").GetString(),
                TemperatureCelsius = root.GetProperty("current").GetProperty("temp_c").GetDouble(),
                Description = root.GetProperty("current").GetProperty("condition").GetProperty("text").GetString()
            };
        }
    }


//defines a class to hold the weather data
//this class is used to deserialize the JSON response from the API
    public class WeatherResult
    {
        public string? City { get; set; }
        public string? Country { get; set; }
        public double TemperatureCelsius { get; set; }
        public string? Description { get; set; }
    }
}
