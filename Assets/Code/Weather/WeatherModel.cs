namespace Weather
{
    public class WeatherModel
    {
        private string _name;
        private string _temperature;
        private string _temperatureUnit;
        private string _windSpeed;
        private string _windDirection;
        private string _iconUrl;
        private string _shortForecast;
        private string _detailedForecast;
        private bool _isDaytime;

        public string Name { get => _name; set => _name = value; }
        public string Temperature { get => _temperature; set => _temperature = value; }
        public string TemperatureUnit { get => _temperatureUnit; set => _temperatureUnit = value; }
        public string WindSpeed { get => _windSpeed; set => _windSpeed = value; }
        public string WindDirection { get => _windDirection; set => _windDirection = value; }
        public string IconUrl { get => _iconUrl; set => _iconUrl = value; }
        public string ShortForecast { get => _shortForecast; set => _shortForecast = value; }
        public string DetailedForecast { get => _detailedForecast; set => _detailedForecast = value; }
        public bool IsDaytime { get => _isDaytime; set => _isDaytime = value; }
    }
}

