using UnityEngine;
using Core.Interface;
using UI.Navigation;
using Zenject;
using System;
using Extention;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Web;

namespace Weather
{
    public class WeatherUIPresenter : IInitialisation, ICleanUp
    {
        private DiContainer _di;
        private WeatherEventBus _weatherEventBus;
        private NavigationEventBus _navigationEventBus;
        private PrefabsContainer _prefabsContainer;
        private WeatherModelsContainer _weatherModelsContainer;
        private WeatherPanelView _weatherPanelView;
        private SerializableDictionary<string, Sprite> _iconsCache;
        private GameObject _currentlyActiveOutline;
        private WebRequestsQueue _webRequestsQueue;
        private bool _isInitialized;


        [Inject]
        public void Construct(DiContainer diContainer, WeatherEventBus weatherEventBus,
            PrefabsContainer prefabsContainer, WeatherModelsContainer weatherModelsContainer,
            SceneView sceneView, WebRequestsQueue webRequestsQueue, NavigationEventBus navigationEventBus)
        {
            _di = diContainer;
            _weatherEventBus = weatherEventBus;
            _prefabsContainer = prefabsContainer;
            _weatherModelsContainer = weatherModelsContainer;
            _weatherPanelView = sceneView.WeatherView;
            _webRequestsQueue = webRequestsQueue;
            _navigationEventBus = navigationEventBus;
        }

        public void Initialisation()
        {
            _weatherEventBus.OnFirstInitialization += OnFirstInitialization;
            _weatherEventBus.OnModelUpdated += UpdateUI;
            _navigationEventBus.OnTabChanged += OnTabChanged;
            _iconsCache = new SerializableDictionary<string, Sprite>();
        }

        public void Cleanup()
        {
            _weatherEventBus.OnFirstInitialization -= OnFirstInitialization;
            _weatherEventBus.OnModelUpdated -= UpdateUI;
            _navigationEventBus.OnTabChanged -= OnTabChanged;

            if (_weatherPanelView.DayPanelViews != null)
            {
                foreach (DayPanelView dayView in _weatherPanelView.DayPanelViews)
                    dayView.DayButton.onClick.RemoveAllListeners();
            }
            if (_weatherPanelView.NightPanelViews != null)
            {
                foreach (DayPanelView nightView in _weatherPanelView.NightPanelViews)
                    nightView.DayButton.onClick.RemoveAllListeners();
            }
        }

        private void OnTabChanged(TabType tab)
        {
            if (tab == TabType.Weather && !_isInitialized)
                _weatherPanelView.SetLoadingVisible(true);
        }

        private void OnFirstInitialization()
        {
            _isInitialized = true;
            _weatherPanelView.SetLoadingVisible(false);
            InstantiateWeatherButtons();
        }

        private void InstantiateWeatherButtons()
        {
            _weatherPanelView.Initialize();
            InitializeDayPanel().Forget();
            InitializeNightPanel().Forget();
        }

        private async UniTaskVoid InitializeDayPanel()
        {
            foreach (WeatherModel model in _weatherModelsContainer.DayWeatherModels)
            {
                GameObject dayPanelObject =
                    _di.InstantiatePrefab(_prefabsContainer.DayButtonPrefab, _weatherPanelView.DaySelectorRect);
                DayPanelView dayPanelView = dayPanelObject.GetComponent<DayPanelView>();
                await SetButtonParametrs(model, dayPanelView);
                dayPanelView.DayButton.onClick.AddListener(delegate { ShowDayInfo(model, dayPanelView); });
                _weatherPanelView.DayPanelViews.Add(dayPanelView);
            }
            ShowDayInfo(_weatherModelsContainer.DayWeatherModels[0],
                _weatherPanelView.DayPanelViews[0]);
            _weatherEventBus.OnSceneContentInitialized?.Invoke();

        }

        private async UniTaskVoid InitializeNightPanel()
        {
            foreach (WeatherModel model in _weatherModelsContainer.NightWeatherModels)
            {
                GameObject dayPanelObject =
                    _di.InstantiatePrefab(_prefabsContainer.DayButtonPrefab, _weatherPanelView.NightSelectorRect);
                DayPanelView dayPanelView = dayPanelObject.GetComponent<DayPanelView>();
                await SetButtonParametrs (model, dayPanelView);
                dayPanelView.DayButton.onClick.AddListener(delegate { ShowDayInfo(model, dayPanelView); });
                _weatherPanelView.NightPanelViews.Add(dayPanelView);
            }
        }

        private async UniTaskVoid UpdateDayPanel()
        {
            if (_weatherModelsContainer.DayWeatherModels.Count != 
                _weatherPanelView.DayPanelViews.Count)
            {
                Debug.Log($"Models count error");
                return;
            }
            for (int i = 0; i < _weatherModelsContainer.DayWeatherModels.Count; i++)
            {
                WeatherModel model = _weatherModelsContainer.DayWeatherModels[i];
                DayPanelView dayPanelView = _weatherPanelView.DayPanelViews[i];
                await SetButtonParametrs(model, dayPanelView);
            }
        }

        private async UniTaskVoid UpdateNightPanel()
        {
            if (_weatherModelsContainer.NightWeatherModels.Count !=
                _weatherPanelView.NightPanelViews.Count)
            {
                Debug.Log("Models count error");
                return;
            }
            for (int i = 0; i < _weatherModelsContainer.NightWeatherModels.Count; i++)
            {
                WeatherModel model = _weatherModelsContainer.NightWeatherModels[i];
                DayPanelView dayPanelView = _weatherPanelView.NightPanelViews[i];
                await SetButtonParametrs(model, dayPanelView);
            }
        }

        private void ShowDayInfo(WeatherModel model, DayPanelView dayPanelView)
        {
            _weatherPanelView.TitleText.text = model.Name;
            _weatherPanelView.DescriptionText.text = model.DetailedForecast;
            ChangeImage(_weatherPanelView.Icon, model.IconUrl).Forget();
            ShowButtonOutline(dayPanelView);
        }

        private async UniTaskVoid ChangeImage(Image icon, string iconUrl)
        {
            await SetImage(icon, iconUrl);
        }

        private void ShowButtonOutline(DayPanelView dayPanelView)
        {
            if (_currentlyActiveOutline != null)
            {
                _currentlyActiveOutline.SetActive(false);
            }
            dayPanelView.OutlinePanel.SetActive(true);
            _currentlyActiveOutline = dayPanelView.OutlinePanel;
        }

        private void UpdateUI()
        {
            UpdateDayPanel().Forget();
            UpdateNightPanel().Forget();
        }

        private async UniTask SetButtonParametrs(WeatherModel model, DayPanelView dayPanelView)
        {
            dayPanelView.NameText.text = model.Name;
            dayPanelView.ShortInfoText.text = model.ShortForecast;
            dayPanelView.TemperatureText.text = $"{model.Temperature} {model.TemperatureUnit}";
            await SetImage(dayPanelView.Icon, model.IconUrl);

        }

        private async UniTask SetImage(Image image, string iconUrl)
        {
            if (_iconsCache.IsContainsKey(iconUrl))
            {
                image.sprite = _iconsCache.GetValue(iconUrl);
            }
            else
            {
                await DownloadIconSprite(image, iconUrl);
            }
        }

        private async UniTask DownloadIconSprite(Image image, string iconUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(iconUrl);

            await UniTaskExtentded.SendWebRequestSafely(request);

            Texture2D texture2D = DownloadHandlerTexture.GetContent(request) as Texture2D;
            Sprite iconSprite = 
                Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            _iconsCache.Add(iconUrl, iconSprite);
            request.Dispose();
            image.sprite = iconSprite;
        }
    }
}
