using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Weather
{
    public class WeatherPanelView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private RectTransform _daySelectorRect;
        [SerializeField] private RectTransform _nightSelectorRect;
        [SerializeField] private GameObject _loadingIndicator;
        private List<DayPanelView> _dayPanelViews;
        private List<DayPanelView> _nightPanelViews;

        public Image Icon { get => _icon; set => _icon = value; }
        public TMP_Text TitleText { get => _titleText; set => _titleText = value; }
        public TMP_Text DescriptionText { get => _descriptionText; set => _descriptionText = value; }
        public RectTransform DaySelectorRect { get => _daySelectorRect; set => _daySelectorRect = value; }
        public RectTransform NightSelectorRect { get => _nightSelectorRect; set => _nightSelectorRect = value; }
        public List<DayPanelView> DayPanelViews { get => _dayPanelViews; set => _dayPanelViews = value; }
        public List<DayPanelView> NightPanelViews { get => _nightPanelViews; set => _nightPanelViews = value; }

        public void SetLoadingVisible(bool visible)
        {
            if (_loadingIndicator != null)
                _loadingIndicator.SetActive(visible);
        }

        public void Initialize()
        {
            _dayPanelViews = new List<DayPanelView>();
            _nightPanelViews = new List<DayPanelView>();
        }
    }
}

