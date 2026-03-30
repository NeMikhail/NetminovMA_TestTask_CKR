using Clicker;
using DogFacts;
using UI.Navigation;
using UnityEngine;
using Web;
using Weather;
using Zenject;

namespace Core
{
    public class GameFactory
    {
        private Presenters _presenters;
        private DiContainer _di;

        [Inject]
        public void Construct(DiContainer di, Presenters presenters)
        {
            _di = di;
            _presenters = presenters;
        }

        public void Init()
        {
            InitializeWebModule();
            InitializeScene();
            InitializeNavigationModule();
            InitializeClickerModule();
            InitializeWeatherModule();
            InitializeDogsModule();
            InitializeLoadingScreenModule();
        }

        private void InitializeWebModule()
        {
            WebRequestsManager webQueueManager = _di.Resolve<WebRequestsManager>();
            _presenters.Add(webQueueManager);
        }

        private void InitializeScene()
        {
            GameObject canvas = _di.InstantiatePrefab(_di.Resolve<PrefabsContainer>().ScenePrefab);
            SceneView sceneView = canvas.GetComponent<SceneView>();
            _di.Bind<SceneView>().FromInstance(sceneView).AsSingle();
        }

        private void InitializeNavigationModule()
        {
            NavigationPresenter navigationPresenter = _di.Resolve<NavigationPresenter>();
            _presenters.Add(navigationPresenter);
        }

        private void InitializeClickerModule()
        {
            ClickerPanelView clickerPanelView = _di.Resolve<SceneView>().ClickerView;
            _di.Bind<ClickerPanelView>().FromInstance(clickerPanelView).AsSingle();

            ClickerPresenter clickerPresenter = _di.Resolve<ClickerPresenter>();
            _presenters.Add(clickerPresenter);

            ClickerUIPresenter clickerUIPresenter = _di.Resolve<ClickerUIPresenter>();
            _presenters.Add(clickerUIPresenter);
        }

        private void InitializeWeatherModule()
        {
            WeatherPresenter weatherPresenter = _di.Resolve<WeatherPresenter>();
            _presenters.Add(weatherPresenter);
            WeatherUIPresenter weatherUIPresenter = _di.Resolve<WeatherUIPresenter>();
            _presenters.Add(weatherUIPresenter);
        }

        private void InitializeDogsModule()
        {
            DogFactsPresenter dogFactsPresenter = _di.Resolve<DogFactsPresenter>();
            _presenters.Add(dogFactsPresenter);
            DogFactsUIPresenter dogFactsUIPresenter = _di.Resolve<DogFactsUIPresenter>();
            _presenters.Add(dogFactsUIPresenter);
        }

        private void InitializeLoadingScreenModule()
        {
            LoadingPresenter loadingPresenter = _di.Resolve<LoadingPresenter>();
            _presenters.Add(loadingPresenter);
        }
    }
}
