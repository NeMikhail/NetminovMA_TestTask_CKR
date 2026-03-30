using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Core.Interface;
using UI.Navigation;
using UniRx;
using Web;
using Zenject;

namespace Weather
{
    public class WeatherPresenter : IInitialisation, ICleanUp
    {
        private const string WEATHER_API_URL = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        private const float TIMER_DELAY = 5f;

        private WeatherEventBus _weatherEventBus;
        private WeatherModelsContainer _weatherModels;
        private APIRequester _api;
        private WebRequestsQueue _webRequestsQueue;
        private NavigationEventBus _navigationEventBus;

        private bool _isFirstRequest;
        private bool _isWeatherTabActive;
        private CompositeDisposable _disposables;
        private WebRequestToken _currentToken;

        [Inject]
        public void Construct(
            WeatherEventBus weatherEventBus,
            WeatherModelsContainer weatherModels,
            APIRequester api,
            WebRequestsQueue webRequestsQueue,
            NavigationEventBus navigationEventBus)
        {
            _api = api;
            _weatherEventBus = weatherEventBus;
            _weatherModels = weatherModels;
            _webRequestsQueue = webRequestsQueue;
            _navigationEventBus = navigationEventBus;
        }

        public void Initialisation()
        {
            _isFirstRequest = true;
            _navigationEventBus.OnTabChanged += OnTabChanged;
            _isWeatherTabActive = _navigationEventBus.CurrentTab == TabType.Weather;
        }

        public void Cleanup()
        {
            _navigationEventBus.OnTabChanged -= OnTabChanged;
            CancelCurrentRequest();
            _disposables?.Dispose();
        }

        private void OnTabChanged(TabType tab)
        {
            if (tab == TabType.Weather)
            {
                _isWeatherTabActive = true;
                StartWeatherTimer();
            }
            else
            {
                _isWeatherTabActive = false;
                CancelCurrentRequest();
                _disposables?.Dispose();
                _disposables = null;
            }
        }

        private void StartWeatherTimer()
        {
            _disposables?.Dispose();
            _disposables = new CompositeDisposable();

            AsyncInit().Forget();
        }

        private async UniTaskVoid AsyncInit()
        {
            await GetWeatherInfo();

            if (!_isWeatherTabActive)
                return;

            Observable.Timer(TimeSpan.FromSeconds(TIMER_DELAY))
                .Repeat()
                .Subscribe(_ =>
                {
                    if (_isWeatherTabActive)
                        GetWeatherInfo().Forget();
                })
                .AddTo(_disposables);
        }

        private async UniTask GetWeatherInfo()
        {
            await RequestWeather();
        }

        private async UniTask RequestWeather()
        {
            try
            {
                var (task, token) = _api.GetWeatherData(WEATHER_API_URL);
                _currentToken = token;
                List<WeatherModel> weatherModels = await task;
                _currentToken = null;
                UpdateModels(weatherModels);

                if (_isFirstRequest)
                {
                    _isFirstRequest = false;
                    _weatherEventBus.OnFirstInitialization?.Invoke();
                }
                else
                {
                    _weatherEventBus.OnModelUpdated?.Invoke();
                }
            }
            catch (OperationCanceledException)
            {
                _currentToken = null;
            }
        }

        private void CancelCurrentRequest()
        {
            if (_currentToken != null && !_currentToken.IsCancelled)
            {
                _currentToken.Cancel();
                _webRequestsQueue.TryRemove(_currentToken);
                _currentToken = null;
            }
        }

        private void UpdateModels(List<WeatherModel> weatherModels)
        {
            _weatherModels.ClearModels();
            foreach (WeatherModel model in weatherModels)
            {
                if (model.IsDaytime)
                    _weatherModels.DayWeatherModels.Add(model);
                else
                    _weatherModels.NightWeatherModels.Add(model);
            }
        }
    }
}
