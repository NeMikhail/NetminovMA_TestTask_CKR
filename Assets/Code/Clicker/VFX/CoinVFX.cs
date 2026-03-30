using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Clicker.VFX
{
    public class CoinVFX : MonoBehaviour, IPoolable<IMemoryPool>
    {
        [SerializeField] private float _flyDuration = 0.7f;
        [SerializeField] private float _arcHeight = 150f;

        private IMemoryPool _pool;

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            gameObject.SetActive(true);
        }

        public void Fly(Vector3 targetWorldPos)
        {
            Vector3 startPos = transform.position;
            Vector3 midPos = Vector3.Lerp(startPos, targetWorldPos, 0.5f) + Vector3.up * _arcHeight;

            transform.DOPath(
                new Vector3[] { midPos, targetWorldPos },
                _flyDuration,
                PathType.CatmullRom
            )
            .SetEase(Ease.InOutSine)
            .OnComplete(() => Despawn());
        }

        private void Despawn()
        {
            OnDespawned();
        }

        public void OnDespawned()
        {
            DOTween.Kill(transform);
            gameObject.SetActive(false);
        }
    }
}
