//make web request to weatherapi.com
using System.Net.Http;
//working with JSON
using System.Text.Json;
//synchronous programming
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


//defines a namespace for the service(to organize the code)
namespace WeatherApiWrapper.Services
{
      
    public class WeatherService
    {
        //a private variable to hold the HttpClient instance
        private readonly HttpClient _httpClient;
        private const string ApiKey = "REPLACE_WITH_THINE_API_KEY"; // Replace with your actual key

        //constructor to initialize the HttpClient
        //this is where we inject the HttpClient dependency
        //this allows us to use the HttpClient for making requests
        //the HttpClient is injected through the constructor   

        private readonly string _apiKey;
        public WeatherService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["WeatherApiKey"]!;
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
