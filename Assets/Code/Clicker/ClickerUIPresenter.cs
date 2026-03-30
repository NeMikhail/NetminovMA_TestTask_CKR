using Clicker.VFX;
using Core.Interface;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Clicker
{
    public class ClickerUIPresenter : IInitialisation, ICleanUp
    {
        private ClickerEventBus _clickerEventBus;
        private ClickerModel _clickerModel;
        private ClickerPanelView _clickerPanelView;
        private ClickerVFXPool _vfxPool;

        [Inject]
        public void Construct(
            ClickerEventBus clickerEventBus,
            ClickerModel clickerModel,
            ClickerPanelView clickerPanelView,
            ClickerVFXPool vfxPool)
        {
            _clickerEventBus = clickerEventBus;
            _clickerModel = clickerModel;
            _clickerPanelView = clickerPanelView;
            _vfxPool = vfxPool;
        }

        public void Initialisation()
        {
            _clickerEventBus.OnCurrencyChanged += UpdateCurrencyText;
            _clickerEventBus.OnEnergyChanged += UpdateEnergyText;
            _clickerEventBus.OnClickTriggered += PlayVFX;
            _clickerEventBus.OnAutoCollectTriggered += PlayVFX;
            _clickerEventBus.OnEnergyDepleted += OnEnergyDepleted;
            _clickerEventBus.OnEnergyRestored += OnEnergyRestored;

            _clickerPanelView.ClickButton.onClick.AddListener(OnClickButtonPressed);

            UpdateCurrencyText();
            UpdateEnergyText();
        }

        public void Cleanup()
        {
            _clickerEventBus.OnCurrencyChanged -= UpdateCurrencyText;
            _clickerEventBus.OnEnergyChanged -= UpdateEnergyText;
            _clickerEventBus.OnClickTriggered -= PlayVFX;
            _clickerEventBus.OnAutoCollectTriggered -= PlayVFX;
            _clickerEventBus.OnEnergyDepleted -= OnEnergyDepleted;
            _clickerEventBus.OnEnergyRestored -= OnEnergyRestored;

            _clickerPanelView.ClickButton.onClick.RemoveAllListeners();
        }

        private void OnClickButtonPressed()
        {
            _clickerEventBus.OnClickTriggered?.Invoke();
        }

        private void UpdateCurrencyText()
        {
            _clickerPanelView.CurrencyText.text = _clickerModel.Currency.ToString();
        }

        private void UpdateEnergyText()
        {
            _clickerPanelView.EnergyText.text = $"{_clickerModel.Energy} / {_clickerModel.MaxEnergy}";
        }

        private void PlayVFX()
        {
            // 4.1 Particle scatter
            Vector3 buttonWorldPos = _clickerPanelView.ButtonRect.position;
            _vfxPool.SpawnParticle(buttonWorldPos);

            // 4.2 Coin fly to currency text
            Vector3 target = _clickerPanelView.CurrencyText.transform.position;
            _vfxPool.SpawnCoin(_clickerPanelView.CoinSpawnPoint.parent, _clickerPanelView.CoinSpawnPoint.localPosition, target);

            // 4.3 Button scale punch (kill previous tween first to prevent scale stacking)
            DOTween.Kill(_clickerPanelView.ButtonRect);
            _clickerPanelView.ButtonRect.localScale = Vector3.one;
            _clickerPanelView.ButtonRect.DOPunchScale(Vector3.one * 0.15f, 0.2f, 5, 0.5f);

            // 4.4 Sound
            if (_clickerPanelView.ClickSound != null)
                _clickerPanelView.AudioSource.PlayOneShot(_clickerPanelView.ClickSound);
        }

        private void OnEnergyDepleted()
        {
            var colors = _clickerPanelView.ClickButton.colors;
            colors.normalColor = new Color(0.6f, 0.6f, 0.6f, 1f);
            _clickerPanelView.ClickButton.colors = colors;
        }

        private void OnEnergyRestored()
        {
            var colors = _clickerPanelView.ClickButton.colors;
            colors.normalColor = Color.white;
            _clickerPanelView.ClickButton.colors = colors;
        }
    }
}
