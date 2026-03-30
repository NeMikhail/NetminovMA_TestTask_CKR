using UnityEngine;
using Zenject;

namespace Clicker.VFX
{
    public class ClickerVFXPool
    {
        private MemoryPool<ClickParticleVFX> _particlePool;
        private MemoryPool<CoinVFX> _coinPool;

        [Inject]
        public void Construct(
            MemoryPool<ClickParticleVFX> particlePool,
            MemoryPool<CoinVFX> coinPool)
        {
            _particlePool = particlePool;
            _coinPool = coinPool;
        }

        public ClickParticleVFX SpawnParticle(Vector3 worldPosition)
        {
            ClickParticleVFX vfx = _particlePool.Spawn();
            vfx.OnSpawned(_particlePool);
            vfx.transform.position = worldPosition;
            return vfx;
        }

        public CoinVFX SpawnCoin(Transform parent, Vector3 localPosition, Vector3 targetWorldPos)
        {
            CoinVFX coin = _coinPool.Spawn();
            coin.transform.SetParent(parent, false);
            coin.transform.localPosition = localPosition;
            coin.OnSpawned(_coinPool);
            coin.Fly(targetWorldPos);
            return coin;
        }
    }
}
