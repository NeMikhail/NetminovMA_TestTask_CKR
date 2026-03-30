using System;

namespace Clicker
{
    public class ClickerEventBus
    {
        public Action OnCurrencyChanged;
        public Action OnEnergyChanged;
        public Action OnClickTriggered;
        public Action OnAutoCollectTriggered;
        public Action OnEnergyDepleted;
        public Action OnEnergyRestored;
    }
}
