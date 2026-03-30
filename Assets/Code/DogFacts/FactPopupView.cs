using TMPro;
using UnityEngine;

public class FactPopupView : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private RectTransform _PopupPanelRect;

    public TMP_Text NameText { get => _nameText; }
    public TMP_Text DescriptionText { get => _descriptionText; }
    public RectTransform PopupPanelRect { get => _PopupPanelRect; }
}
