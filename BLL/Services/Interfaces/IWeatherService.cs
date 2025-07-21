using Models.Contracts.Weather;

namespace BLL.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<TodaysWeatherResponse> GetTodaysWeather();
    }
}