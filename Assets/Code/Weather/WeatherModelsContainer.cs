using System.Collections.Generic;

namespace Weather
{
    public class WeatherModelsContainer
    {
        private List<WeatherModel> _dayWeatherModels;
        private List<WeatherModel> _nightWeatherModels;

        public List<WeatherModel> DayWeatherModels { get => _dayWeatherModels; set => _dayWeatherModels = value; }
        public List<WeatherModel> NightWeatherModels { get => _nightWeatherModels; set => _nightWeatherModels = value; }


        public WeatherModelsContainer()
        {
            _dayWeatherModels = new List<WeatherModel>();
            _nightWeatherModels = new List<WeatherModel>();
        }

        public void ClearModels()
        {
            _dayWeatherModels.Clear();
            _nightWeatherModels.Clear();
        }
    }
}
