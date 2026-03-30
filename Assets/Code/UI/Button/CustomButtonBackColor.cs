using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButtonBackColor : CustomButtonBase
{
    private Color _originalColor;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Color _toColor = Color.black;
    [SerializeField] private float _duration;


    private void Awake()
    {
        _originalColor = _buttonImage.color;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        _buttonImage.DOColor(_toColor, _duration)
            .SetEase(Ease.InOutSine);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        _buttonImage.DOColor(_originalColor, _duration)
            .SetEase(Ease.InOutSine);
    }
}
