using System.Collections.Generic;
using UnityEngine;

namespace DogFacts
{
    public class FactsPanelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _factsScrollRect;
        [SerializeField] private FactPopupView _factPopupView;
        [SerializeField] private GameObject _loadingIndicator;

        private List<FactButtonView> _factButtonViews;

        public RectTransform FactsScrollRect => _factsScrollRect;
        public FactPopupView FactPopupView => _factPopupView;
        public List<FactButtonView> FactButtonViews { get => _factButtonViews; set => _factButtonViews = value; }

        public void Initialize()
        {
            _factButtonViews = new List<FactButtonView>();
        }

        public void SetLoadingVisible(bool visible)
        {
            if (_loadingIndicator != null)
                _loadingIndicator.SetActive(visible);
        }
    }
}
