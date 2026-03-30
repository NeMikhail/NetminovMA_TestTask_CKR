using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DogFacts
{
    public class FactButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _numberText;
        [SerializeField] private TMP_Text _factNameText;

        public Button Button { get => _button; set => _button = value; }
        public TMP_Text NumberText { get => _numberText; }
        public TMP_Text FactNameText { get => _factNameText; }
    }
}

