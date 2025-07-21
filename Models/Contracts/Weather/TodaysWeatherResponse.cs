namespace Models.Contracts.Weather
{
    public class TodaysWeatherResponse : Response
    {
        public int Temprature { get; set; }
        public string CloudCoverage { get; set; } = String.Empty;
        public int RainCoverage { get; set; }
    }
}