using BLL.Services.Interfaces;
using Models.Contracts.Weather;

namespace BLL.Services
{
    public class WeatherService : IWeatherService
    {
        public async Task<TodaysWeatherResponse> GetTodaysWeather()
        {
            // Simulate asynchronous work
            await Task.Delay(500);

            TodaysWeatherResponse response = new()
            {
            Temprature = 21,
            CloudCoverage = "Partly Cloudy",
            RainCoverage = 16,
            IsSuccess = true,
            StatusCode = 200,
            StatusMessage = "OK"
            };

            return response;
        }
    }
}