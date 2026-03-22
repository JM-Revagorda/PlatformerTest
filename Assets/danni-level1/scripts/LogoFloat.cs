using UnityEngine;
using DG.Tweening;

public class LogoFloat : MonoBehaviour
{
    [Header("Settings")]
    public float floatDistance = 15f; // How high it bobs
    public float duration = 2f;      // How long one "bob" takes
    public float delayPerLetter = 0.1f; // The "Wave" secret

    void Start()
    {
        // 1. Get the letter's position in the hierarchy (0, 1, 2...)
        int index = transform.GetSiblingIndex();

        // 2. Calculate a delay based on its position
        float startDelay = index * delayPerLetter;

        // 3. Start the floating animation with a delay
        // Move local Y up, then Yoyo back and forth forever
        transform.DOLocalMoveY(transform.localPosition.y + floatDistance, duration)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetDelay(startDelay)
                 .SetUpdate(true);
    }
}