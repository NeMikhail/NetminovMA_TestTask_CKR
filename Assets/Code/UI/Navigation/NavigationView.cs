using UnityEngine;
using UnityEngine.UI;

namespace UI.Navigation
{
    public class NavigationView : MonoBehaviour
    {
        [SerializeField] private Button _clickerTabButton;
        [SerializeField] private Button _weatherTabButton;
        [SerializeField] private Button _dogsTabButton;

        public Button ClickerTabButton => _clickerTabButton;
        public Button WeatherTabButton => _weatherTabButton;
        public Button DogsTabButton => _dogsTabButton;
    }
}
