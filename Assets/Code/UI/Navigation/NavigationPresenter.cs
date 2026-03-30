using Core.Interface;
using UnityEngine;
using Zenject;

namespace UI.Navigation
{
    public class NavigationPresenter : IInitialisation, ICleanUp
    {
        private NavigationEventBus _navigationEventBus;
        private SceneView _sceneView;

        [Inject]
        public void Construct(NavigationEventBus navigationEventBus, SceneView sceneView)
        {
            _navigationEventBus = navigationEventBus;
            _sceneView = sceneView;
        }

        public void Initialisation()
        {
            NavigationView navView = _sceneView.NavigationView;
            navView.ClickerTabButton.onClick.AddListener(() => OnTabClicked(TabType.Clicker));
            navView.WeatherTabButton.onClick.AddListener(() => OnTabClicked(TabType.Weather));
            navView.DogsTabButton.onClick.AddListener(() => OnTabClicked(TabType.DogFacts));

            _navigationEventBus.OnTabChanged += OnTabChanged;

            _navigationEventBus.SwitchTab(TabType.Clicker);
        }

        public void Cleanup()
        {
            _navigationEventBus.OnTabChanged -= OnTabChanged;

            NavigationView navView = _sceneView.NavigationView;
            navView.ClickerTabButton.onClick.RemoveAllListeners();
            navView.WeatherTabButton.onClick.RemoveAllListeners();
            navView.DogsTabButton.onClick.RemoveAllListeners();
        }

        private void OnTabClicked(TabType tab)
        {
            if (_navigationEventBus.CurrentTab != tab)
                _navigationEventBus.SwitchTab(tab);
        }

        private void OnTabChanged(TabType tab)
        {
            _sceneView.ShowTab(tab);
        }
    }
}
