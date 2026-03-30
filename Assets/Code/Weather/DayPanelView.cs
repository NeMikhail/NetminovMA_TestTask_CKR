using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Weather
{
    public class DayPanelView : MonoBehaviour
    {
        [SerializeField] private Button _dayButton;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _shortInfoText;
        [SerializeField] private TMP_Text _temperatureText;
        [SerializeField] private GameObject _outlinePanel;

        public Button DayButton { get => _dayButton; }
        public Image Icon { get => _icon; set => _icon = value; }
        public TMP_Text NameText { get => _nameText; }
        public TMP_Text ShortInfoText { get => _shortInfoText; }
        public TMP_Text TemperatureText { get => _temperatureText; }
        public GameObject OutlinePanel { get => _outlinePanel; set => _outlinePanel = value; }
    }
}

