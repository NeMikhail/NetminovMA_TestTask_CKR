using Core.Interface;
using Cysharp.Threading.Tasks;
using System;
using UI.Navigation;
using UnityEngine;
using Web;
using Zenject;
using Extention;

namespace DogFacts
{
    public class DogFactsUIPresenter : IInitialisation, ICleanUp
    {
        private const float POPUP_STRING_LENGTH = 34f;
        private const float POPUP_STRING_HEIGHT = 40f;
        private const float POPUP_BASIC_HEIGHT = 200f;

        private DiContainer _di;
        private PrefabsContainer _prefabsContainer;
        private FactsModelsContainer _factsModels;
        private DogFactsEventBus _dogFactsEventBus;
        private FactsPanelView _dogsPanelView;
        private APIRequester _api;
        private WebRequestsQueue _webRequestsQueue;
        private NavigationEventBus _navigationEventBus;

        private SerializableDictionary<FactButtonView, DogFactModel> _factButtonsDict;
        private WebRequestToken _currentBreedToken;

        [Inject]
        public void Construct(
            DiContainer diContainer,
            PrefabsContainer prefabsContainer,
            FactsModelsContainer factsModels,
            DogFactsEventBus dogFactsEventBus,
            SceneView sceneView,
            APIRequester api,
            WebRequestsQueue webRequestsQueue,
            NavigationEventBus navigationEventBus)
        {
            _di = diContainer;
            _prefabsContainer = prefabsContainer;
            _factsModels = factsModels;
            _dogFactsEventBus = dogFactsEventBus;
            _dogsPanelView = sceneView.FactsView;
            _api = api;
            _webRequestsQueue = webRequestsQueue;
            _navigationEventBus = navigationEventBus;
        }

        public void Initialisation()
        {
            _dogFactsEventBus.OnDogModelsLoaded += CreateUI;
            _factButtonsDict = new SerializableDictionary<FactButtonView, DogFactModel>();
            _dogsPanelView.Initialize();
            _navigationEventBus.OnTabChanged += OnTabChanged;
        }

        public void Cleanup()
        {
            _dogFactsEventBus.OnDogModelsLoaded -= CreateUI;
            _navigationEventBus.OnTabChanged -= OnTabChanged;
            foreach (FactButtonView factButtonView in _dogsPanelView.FactButtonViews)
            {
                factButtonView.Button.onClick.RemoveAllListeners();
            }
            CancelCurrentBreedRequest();
        }

        private bool _listLoaded;

        private void OnTabChanged(TabType tab)
        {
            if (tab == TabType.DogFacts)
            {
                if (!_listLoaded)
                    _dogsPanelView.SetLoadingVisible(true);
            }
            else
            {
                CancelCurrentBreedRequest();
                _dogsPanelView.SetLoadingVisible(false);
            }
        }

        private void CreateUI()
        {
            _listLoaded = true;
            _dogsPanelView.SetLoadingVisible(false);
            int index = 1;
            int count = 0;
            foreach (DogFactModel model in _factsModels.DogFactModels)
            {
                if (count >= 10) break;

                GameObject dogButtonObject =
                    _di.InstantiatePrefab(_prefabsContainer.DogButtonPrefab, _dogsPanelView.FactsScrollRect);
                FactButtonView factView = dogButtonObject.GetComponent<FactButtonView>();
                SetButtonParameters(model, factView, index);
                _dogsPanelView.FactButtonViews.Add(factView);
                _factButtonsDict.Add(factView, model);
                index++;
                count++;
            }
        }

        private void SetButtonParameters(DogFactModel model, FactButtonView factView, int index)
        {
            factView.FactNameText.text = model.Name;
            factView.NumberText.text = index.ToString();
            factView.Button.onClick.AddListener(delegate { OnBreedButtonClicked(factView); });
        }

        private void OnBreedButtonClicked(FactButtonView factButtonView)
        {
            CancelCurrentBreedRequest();
            _dogsPanelView.SetLoadingVisible(true);
            _dogsPanelView.FactPopupView.gameObject.SetActive(false);

            DogFactModel model = _factButtonsDict.GetValue(factButtonView);
            LoadBreedDetails(model.Id).Forget();
        }

        private async UniTask LoadBreedDetails(string breedId)
        {
            try
            {
                var (task, token) = _api.GetDogBreedById(breedId);
                _currentBreedToken = token;
                DogFactModel model = await task;
                _currentBreedToken = null;
                _dogsPanelView.SetLoadingVisible(false);
                ShowDogPopUp(model);
            }
            catch (OperationCanceledException)
            {
                _currentBreedToken = null;
                _dogsPanelView.SetLoadingVisible(false);
            }
        }

        private void ShowDogPopUp(DogFactModel model)
        {
            FactPopupView popupView = _dogsPanelView.FactPopupView;
            int stringsCount = (int)(model.Description.Length / POPUP_STRING_LENGTH);
            popupView.PopupPanelRect.sizeDelta =
                new Vector2(
                    popupView.PopupPanelRect.sizeDelta.x,
                    POPUP_BASIC_HEIGHT + (stringsCount * POPUP_STRING_HEIGHT));
            popupView.NameText.text = model.Name;
            popupView.DescriptionText.text = model.Description;
            popupView.gameObject.SetActive(true);
        }

        private void CancelCurrentBreedRequest()
        {
            if (_currentBreedToken != null && !_currentBreedToken.IsCancelled)
            {
                _webRequestsQueue.TryRemove(_currentBreedToken);
                _currentBreedToken.Cancel();
                _currentBreedToken = null;
            }
        }
    }
}
