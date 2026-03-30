using System.Collections;
using UnityEngine;
using Zenject;

namespace Clicker.VFX
{
    public class ClickParticleVFX : MonoBehaviour, IPoolable<IMemoryPool>
    {
        private ParticleSystem _particleSystem;
        private IMemoryPool _pool;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _particleSystem.Play();
            StartCoroutine(ReturnAfterDelay());
        }

        public void OnDespawned()
        {
            StopAllCoroutines();
            _particleSystem.Stop();
            _particleSystem.Clear();
        }

        private IEnumerator ReturnAfterDelay()
        {
            var main = _particleSystem.main;
            float duration = main.duration + main.startLifetime.constantMax;
            yield return new WaitForSeconds(duration);
            _pool.Despawn(this);
        }
    }
}
