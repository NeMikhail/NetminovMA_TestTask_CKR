using Core.Interface;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UI.Navigation;
using Web;
using Zenject;

namespace DogFacts
{
    public class DogFactsPresenter : IInitialisation, ICleanUp
    {
        private const string DOGS_API_URL = "https://dogapi.dog/api/v2/breeds";

        private APIRequester _api;
        private FactsModelsContainer _factsModels;
        private DogFactsEventBus _dogFactsEventBus;
        private WebRequestsQueue _webRequestsQueue;
        private NavigationEventBus _navigationEventBus;

        private bool _isFirstEntry = true;
        private WebRequestToken _listToken;

        [Inject]
        public void Construct(
            FactsModelsContainer factsModels,
            APIRequester api,
            DogFactsEventBus dogFactsEventBus,
            WebRequestsQueue webRequestsQueue,
            NavigationEventBus navigationEventBus)
        {
            _api = api;
            _factsModels = factsModels;
            _dogFactsEventBus = dogFactsEventBus;
            _webRequestsQueue = webRequestsQueue;
            _navigationEventBus = navigationEventBus;
        }

        public void Initialisation()
        {
            _navigationEventBus.OnTabChanged += OnTabChanged;
        }

        public void Cleanup()
        {
            _navigationEventBus.OnTabChanged -= OnTabChanged;
            CancelListRequest();
        }

        private void OnTabChanged(TabType tab)
        {
            if (tab == TabType.DogFacts)
            {
                if (_isFirstEntry)
                {
                    _isFirstEntry = false;
                    GetDogFacts().Forget();
                }
            }
            else
            {
                CancelListRequest();
            }
        }

        public void CancelListRequest()
        {
            if (_listToken != null && !_listToken.IsCancelled)
            {
                _listToken.Cancel();
                _webRequestsQueue.TryRemove(_listToken);
                _listToken = null;
            }
        }

        private async UniTask GetDogFacts()
        {
            try
            {
                var (task, token) = _api.GetDogsData(DOGS_API_URL);
                _listToken = token;
                List<DogFactModel> dogFactsList = await task;
                _listToken = null;

                foreach (DogFactModel model in dogFactsList)
                {
                    _factsModels.DogFactModels.Add(model);
                }

                _dogFactsEventBus.OnDogModelsLoaded?.Invoke();
            }
            catch (OperationCanceledException)
            {
                _listToken = null;
            }
        }
    }
}
