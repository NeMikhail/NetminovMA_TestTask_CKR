using Clicker;
using Clicker.VFX;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private PrefabsContainer _prefabsContainer;
    [SerializeField] private ClickerConfig _clickerConfig;

    public override void InstallBindings()
    {
        Container.Bind<PrefabsContainer>().FromInstance(_prefabsContainer).AsSingle();
        Container.Bind<ClickerConfig>().FromInstance(_clickerConfig).AsSingle();

        BindClickerPools();
    }

    private void BindClickerPools()
    {
        if (_prefabsContainer.ClickParticlePrefab != null)
        {
            Container.BindMemoryPool<ClickParticleVFX, MemoryPool<ClickParticleVFX>>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_prefabsContainer.ClickParticlePrefab)
                .UnderTransformGroup("ClickerVFXPool");
        }

        if (_prefabsContainer.CoinVFXPrefab != null)
        {
            Container.BindMemoryPool<CoinVFX, MemoryPool<CoinVFX>>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_prefabsContainer.CoinVFXPrefab)
                .UnderTransformGroup("ClickerVFXPool");
        }
    }
}
