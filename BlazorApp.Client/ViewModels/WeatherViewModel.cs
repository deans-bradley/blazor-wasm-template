using BlazorApp.Client.ViewModels.Interfaces;
using BlazorApp.Utils.Services;
using Models.Contracts.Weather;

namespace BlazorApp.Client.ViewModels
{
    public class WeatherViewModel(IHttpService httpService) : IWeatherViewModel
    {
        private readonly IHttpService _httpServie = httpService;

        public async Task<TodaysWeatherResponse> GetTodaysWeather()
        {
            TodaysWeatherResponse response = await _httpServie.Get<TodaysWeatherResponse>("/weather/today");

            return response;
        }
    }
}