using Clicker;
using DogFacts;
using UI.Navigation;
using UnityEngine;
using Weather;

public class SceneView : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private WeatherPanelView _weatherView;
    [SerializeField] private FactsPanelView _factsView;
    [SerializeField] private ClickerPanelView _clickerView;
    [SerializeField] private NavigationView _navigationView;

    public GameObject LoadingScreen => _loadingScreen;
    public WeatherPanelView WeatherView => _weatherView;
    public FactsPanelView FactsView => _factsView;
    public ClickerPanelView ClickerView => _clickerView;
    public NavigationView NavigationView => _navigationView;

    public void ShowTab(TabType tab)
    {
        _clickerView.gameObject.SetActive(tab == TabType.Clicker);
        _weatherView.gameObject.SetActive(tab == TabType.Weather);
        _factsView.gameObject.SetActive(tab == TabType.DogFacts);
    }
}
