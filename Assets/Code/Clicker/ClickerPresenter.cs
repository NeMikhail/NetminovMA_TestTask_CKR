using Core.Interface;
using UI.Navigation;
using Zenject;

namespace Clicker
{
    public class ClickerPresenter : IInitialisation, ICleanUp, IExecute
    {
        private ClickerEventBus _clickerEventBus;
        private ClickerModel _clickerModel;
        private ClickerConfig _clickerConfig;
        private NavigationEventBus _navigationEventBus;

        private bool _isActive;
        private bool _wasEnergyDepleted;
        private float _autoCollectTimer;
        private float _energyRechargeTimer;

        [Inject]
        public void Construct(
            ClickerEventBus clickerEventBus,
            ClickerModel clickerModel,
            ClickerConfig clickerConfig,
            NavigationEventBus navigationEventBus)
        {
            _clickerEventBus = clickerEventBus;
            _clickerModel = clickerModel;
            _clickerConfig = clickerConfig;
            _navigationEventBus = navigationEventBus;
        }

        public void Initialisation()
        {
            _clickerModel.MaxEnergy = _clickerConfig.MaxEnergy;
            _clickerModel.Energy = _clickerConfig.StartEnergy;
            _clickerModel.Currency = 0;

            _navigationEventBus.OnTabChanged += OnTabChanged;
            _clickerEventBus.OnClickTriggered += OnClickTriggered;

            _isActive = _navigationEventBus.CurrentTab == TabType.Clicker;
        }

        public void Execute(float deltaTime)
        {
            if (!_isActive)
                return;

            _autoCollectTimer += deltaTime;
            if (_autoCollectTimer >= _clickerConfig.AutoCollectInterval)
            {
                _autoCollectTimer = 0f;
                TryAutoCollect();
            }

            _energyRechargeTimer += deltaTime;
            if (_energyRechargeTimer >= _clickerConfig.EnergyRechargeInterval)
            {
                _energyRechargeTimer = 0f;
                RechargeEnergy();
            }
        }

        public void Cleanup()
        {
            _navigationEventBus.OnTabChanged -= OnTabChanged;
            _clickerEventBus.OnClickTriggered -= OnClickTriggered;
        }

        private void OnTabChanged(TabType tab)
        {
            _isActive = tab == TabType.Clicker;
        }

        private void OnClickTriggered()
        {
            if (_clickerModel.Energy <= 0)
                return;

            _clickerModel.Energy -= _clickerConfig.EnergyPerClick;
            _clickerModel.Currency += _clickerConfig.CurrencyPerClick;

            _clickerEventBus.OnCurrencyChanged?.Invoke();
            _clickerEventBus.OnEnergyChanged?.Invoke();

            CheckEnergyState();
        }

        private void TryAutoCollect()
        {
            if (_clickerModel.Energy <= 0)
                return;

            _clickerModel.Energy -= _clickerConfig.EnergyPerAutoCollect;
            _clickerModel.Currency += _clickerConfig.CurrencyPerClick;

            _clickerEventBus.OnCurrencyChanged?.Invoke();
            _clickerEventBus.OnEnergyChanged?.Invoke();
            _clickerEventBus.OnAutoCollectTriggered?.Invoke();

            CheckEnergyState();
        }

        private void RechargeEnergy()
        {
            bool wasEmpty = _clickerModel.Energy <= 0;
            _clickerModel.Energy = UnityEngine.Mathf.Min(
                _clickerModel.Energy + _clickerConfig.EnergyRechargeAmount,
                _clickerModel.MaxEnergy);

            _clickerEventBus.OnEnergyChanged?.Invoke();

            if (wasEmpty && _clickerModel.Energy > 0)
            {
                _wasEnergyDepleted = false;
                _clickerEventBus.OnEnergyRestored?.Invoke();
            }
        }

        private void CheckEnergyState()
        {
            if (_clickerModel.Energy <= 0 && !_wasEnergyDepleted)
            {
                _wasEnergyDepleted = true;
                _clickerEventBus.OnEnergyDepleted?.Invoke();
            }
        }
    }
}
