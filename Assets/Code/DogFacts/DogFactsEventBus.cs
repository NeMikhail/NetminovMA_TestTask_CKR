using System;

namespace DogFacts
{
    public class DogFactsEventBus
    {
        private Action _onDogModelsLoaded;
        private Action _onDogFactsUILoaded;

        public Action OnDogModelsLoaded { get => _onDogModelsLoaded; set => _onDogModelsLoaded = value; }
        public Action OnDogFactsUILoaded { get => _onDogFactsUILoaded; set => _onDogFactsUILoaded = value; }
    }
}
