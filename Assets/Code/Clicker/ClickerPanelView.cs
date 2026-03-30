using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker
{
    public class ClickerPanelView : MonoBehaviour
    {
        [SerializeField] private Button _clickButton;
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private TMP_Text _energyText;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private RectTransform _coinSpawnPoint;

        public Button ClickButton => _clickButton;
        public TMP_Text CurrencyText => _currencyText;
        public TMP_Text EnergyText => _energyText;
        public AudioSource AudioSource => _audioSource;
        public AudioClip ClickSound => _clickSound;
        public RectTransform CoinSpawnPoint => _coinSpawnPoint;
        public RectTransform ButtonRect => _clickButton.GetComponent<RectTransform>();
    }
}
