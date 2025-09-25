using Models.Contracts.Weather;

namespace BlazorApp.Client.ViewModels.Interfaces
{
    public interface IWeatherViewModel
    {
        Task<TodaysWeatherResponse> GetTodaysWeather();
    }
}