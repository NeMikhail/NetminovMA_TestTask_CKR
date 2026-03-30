using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CustomButtonScale : CustomButtonBase
{
    private const float OriginalScale = 1.0f;
    [SerializeField] private Transform _scalableObjectTransform;
    [SerializeField] private float _toScale;
    [SerializeField] private float _duration;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        _scalableObjectTransform.DOScale(_toScale, _duration)
            .SetEase(Ease.InOutSine);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        _scalableObjectTransform.DOScale(OriginalScale, _duration)
            .SetEase(Ease.InOutSine);
    }
}
