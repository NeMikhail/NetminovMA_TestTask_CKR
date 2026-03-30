using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButtonBackground : CustomButtonBase
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private float _duration;


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        _backgroundImage.DOFade(1, _duration)
            .SetEase(Ease.InOutSine);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        
        _backgroundImage.DOFade(0, _duration)
            .SetEase(Ease.InOutSine);
    }
}
