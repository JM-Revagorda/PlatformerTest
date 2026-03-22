using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening; // Don't forget this!

public class ButtonJuice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;

        // This creates a "Looping" animation
        // It scales the button up slightly (1.05f) over 2 seconds
        // 'SetLoops(-1, LoopType.Yoyo)' makes it go back and forth forever
        transform.DOScale(initialScale * 1.05f, 2.0f)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetUpdate(true);
    }

    // When mouse hovers over
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Scale up to 1.1x with a "Back" ease (makes it overset and snap back)
        transform.DOScale(initialScale * 1.1f, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    // When mouse leaves
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(initialScale, 0.2f).SetUpdate(true);
    }

    // Call this function via the Button's OnClick() event in the Inspector
    public void Punch()
    {
        // Shakes the button slightly when clicked
        transform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.1f, 10, 1).SetUpdate(true);
    }
}