using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using Weather;
using Zenject;
using DogFacts;

namespace Web
{
    public class APIRequester
    {
        private WebRequestsQueue _webRequestsQueue;

        [Inject]
        public void Construct(WebRequestsQueue webRequestsQueue)
        {
            _webRequestsQueue = webRequestsQueue;
        }

        public (UniTask<List<WeatherModel>> task, WebRequestToken token) GetWeatherData(string apiURL)
        {
            UnityWebRequest request = UnityWebRequest.Get(apiURL);
            WebRequestToken token = _webRequestsQueue.AddRequest(request);
            return (AwaitWeatherData(request, token), token);
        }

        public (UniTask<List<DogFactModel>> task, WebRequestToken token) GetDogsData(string apiURL)
        {
            UnityWebRequest request = UnityWebRequest.Get(apiURL);
            WebRequestToken token = _webRequestsQueue.AddRequest(request);
            return (AwaitDogsData(request, token), token);
        }

        public (UniTask<DogFactModel> task, WebRequestToken token) GetDogBreedById(string breedId)
        {
            string url = $"https://dogapi.dog/api/v2/breeds/{breedId}";
            UnityWebRequest request = UnityWebRequest.Get(url);
            WebRequestToken token = _webRequestsQueue.AddRequest(request);
            return (AwaitBreedData(request, token), token);
        }

        private async UniTask<List<WeatherModel>> AwaitWeatherData(UnityWebRequest request, WebRequestToken token)
        {
            await UniTask.WaitUntil(() => token.IsCompleted || token.IsCancelled);

            if (token.IsCancelled)
                throw new OperationCanceledException();

            string results = token.ResponseText;

            dynamic json = JObject.Parse(results);
            dynamic properties = json["properties"];
            dynamic periods = properties["periods"];

            List<WeatherModel> weatherModels = new List<WeatherModel>();
            foreach (dynamic period in periods)
            {
                WeatherModel model = new WeatherModel();
                model.Name = (string)period["name"];
                model.Temperature = (string)period["temperature"];
                model.TemperatureUnit = (string)period["temperatureUnit"];
                model.WindSpeed = (string)period["windSpeed"];
                model.WindDirection = (string)period["windDirection"];
                model.IconUrl = (string)period["icon"];
                model.ShortForecast = (string)period["shortForecast"];
                model.DetailedForecast = (string)period["detailedForecast"];
                model.IsDaytime = (bool)period["isDaytime"];
                weatherModels.Add(model);
            }

            return weatherModels;
        }

        private async UniTask<List<DogFactModel>> AwaitDogsData(UnityWebRequest request, WebRequestToken token)
        {
            await UniTask.WaitUntil(() => token.IsCompleted || token.IsCancelled);

            if (token.IsCancelled)
                throw new OperationCanceledException();

            string results = token.ResponseText;

            dynamic json = JObject.Parse(results);
            dynamic datas = json["data"];

            List<DogFactModel> dogFactModels = new List<DogFactModel>();
            foreach (dynamic data in datas)
            {
                string id = (string)data["id"];
                dynamic attributes = data["attributes"];
                string name = (string)attributes["name"];
                string description = (string)attributes["description"];
                dogFactModels.Add(new DogFactModel(id, name, description));
            }

            return dogFactModels;
        }

        private async UniTask<DogFactModel> AwaitBreedData(UnityWebRequest request, WebRequestToken token)
        {
            await UniTask.WaitUntil(() => token.IsCompleted || token.IsCancelled);

            if (token.IsCancelled)
                throw new OperationCanceledException();

            string results = token.ResponseText;

            dynamic json = JObject.Parse(results);
            dynamic data = json["data"];
            string id2 = (string)data["id"];
            dynamic attributes = data["attributes"];
            string name = (string)attributes["name"];
            string description = (string)attributes["description"];

            return new DogFactModel(id2, name, description);
        }
    }
}
