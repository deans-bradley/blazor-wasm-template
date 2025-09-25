using BLL.Services.Interfaces;
using FastEndpoints;
using Models.Contracts.Weather;

namespace API.Endpoints.Weather
{
    public class GetCurrentUser(IWeatherService weatherService) : EndpointWithoutRequest<TodaysWeatherResponse>
    {
        private readonly IWeatherService _weatherService = weatherService;

        public override void Configure()
        {
            Get("/weather/today");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            TodaysWeatherResponse response = await _weatherService.GetTodaysWeather();

            await SendAsync(response, cancellation: ct);
        }
    }
}