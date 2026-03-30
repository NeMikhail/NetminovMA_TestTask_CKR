using Clicker;
using Clicker.VFX;
using Core;
using DogFacts;
using UI.Navigation;
using UnityEngine;
using Weather;
using Web;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindCore();
        BindWebModule();
        BindNavigationModule();
        BindClickerModule();
        BindWeatherModule();
        BindDogsModule();
        BindLoadingModule();
    }

    private void BindCore()
    {
        Container.Bind<Presenters>().AsSingle();
        Container.Bind<GameFactory>().AsSingle();
    }

    private void BindWebModule()
    {
        Container.Bind<WebRequestsQueue>().AsSingle();
        Container.Bind<WebRequestsManager>().AsSingle();
        Container.Bind<APIRequester>().AsSingle();
    }

    private void BindNavigationModule()
    {
        Container.Bind<NavigationEventBus>().AsSingle().NonLazy();
        Container.Bind<NavigationPresenter>().AsSingle();
    }

    private void BindClickerModule()
    {
        Container.Bind<ClickerModel>().AsSingle();
        Container.Bind<ClickerEventBus>().AsSingle().NonLazy();
        Container.Bind<ClickerVFXPool>().AsSingle();
        Container.Bind<ClickerPresenter>().AsSingle();
        Container.Bind<ClickerUIPresenter>().AsSingle();
    }

    private void BindWeatherModule()
    {
        Container.Bind<WeatherEventBus>().AsSingle().NonLazy();
        Container.Bind<WeatherModelsContainer>().AsSingle();
        Container.Bind<WeatherPresenter>().AsSingle();
        Container.Bind<WeatherUIPresenter>().AsSingle();
    }

    private void BindDogsModule()
    {
        Container.Bind<DogFactsEventBus>().AsSingle().NonLazy();
        Container.Bind<FactsModelsContainer>().AsSingle();
        Container.Bind<DogFactsPresenter>().AsSingle();
        Container.Bind<DogFactsUIPresenter>().AsSingle();
    }

    private void BindLoadingModule()
    {
        Container.Bind<LoadingPresenter>().AsSingle();
    }
}
