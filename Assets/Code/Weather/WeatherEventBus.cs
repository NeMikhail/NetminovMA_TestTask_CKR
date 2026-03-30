using System;

namespace Weather
{
    public class WeatherEventBus
    {
        private Action _onFirstInitialization;
        private Action _onSceneContentInitialized;
        private Action _onModelUpdated;

        public Action OnFirstInitialization { get => _onFirstInitialization; set => _onFirstInitialization = value; }
        public Action OnSceneContentInitialized { get => _onSceneContentInitialized; set => _onSceneContentInitialized = value; }
        public Action OnModelUpdated { get => _onModelUpdated; set => _onModelUpdated = value; }
        
    }
}
