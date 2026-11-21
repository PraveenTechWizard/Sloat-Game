using System.Collections;
using UnityEngine;

public class ReelController : MonoBehaviour
{
    [Header("Reel References")]
    [SerializeField] private RectTransform reelStrip; // VerticalLayoutGroup container holding symbol images

    [Header("Dimensions & Symbols")]
    [SerializeField] private float symbolHeight = 150f; // Must match each symbol's RectTransform height
    [SerializeField] private int totalUniqueSymbols = 4; // 0: Seven, 1: Cherry, 2: Bell, 3: BAR
    [SerializeField] private float verticalOffset = 0f; // Fine-tune centering (+ moves strip up, - moves down)

    [Header("Animation Settings")]
    [SerializeField] private float spinSpeed = 2200f; // Downward scroll speed in pixels per second
    [SerializeField] private float bounceOffset = 25f; // Undershoot distance before settling on target
    [SerializeField] private float bounceDuration = 0.15f; // Time to ease back into exact alignment

    public bool IsSpinning { get; private set; }
    public int CurrentSymbolIndex { get; private set; }

    private float _loopHeight;
    private Coroutine _spinRoutine;

    private void Start()
    {
        if (reelStrip == null)
        {
            Debug.LogWarning($"[{name}] ReelController is missing 'reelStrip' reference.", this);
            return;
        }

        _loopHeight = symbolHeight * totalUniqueSymbols;

        // 
        SetSymbolInstant(Random.Range(0, totalUniqueSymbols));
    }

    public void SetSymbolInstant(int symbolIndex)
    {
        if (reelStrip == null) return;

        CurrentSymbolIndex = Mathf.Clamp(symbolIndex, 0, totalUniqueSymbols - 1);
        float targetY = GetYForSymbol(CurrentSymbolIndex);
        reelStrip.anchoredPosition = new Vector2(reelStrip.anchoredPosition.x, targetY);
    }

    public void StartSpin(int targetSymbolIndex, float spinDuration)
    {
        if (reelStrip == null || IsSpinning) return;

        CurrentSymbolIndex = Mathf.Clamp(targetSymbolIndex, 0, totalUniqueSymbols - 1);

        if (_spinRoutine != null)
        {
            StopCoroutine(_spinRoutine);
        }

        _spinRoutine = StartCoroutine(SpinRoutine(CurrentSymbolIndex, spinDuration));
    }

    private IEnumerator SpinRoutine(int targetIndex, float duration)
    {
        IsSpinning = true;
        float elapsed = 0f;

        // 
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float nextY = reelStrip.anchoredPosition.y - (spinSpeed * Time.deltaTime);

            // 
            if (nextY <= -_loopHeight * 0.5f)
            {
                nextY += _loopHeight;
            }

            reelStrip.anchoredPosition = new Vector2(reelStrip.anchoredPosition.x, nextY);
            yield return null;
        }

        // 
        float finalY = GetYForSymbol(targetIndex);
        float overshootY = finalY - bounceOffset;
        reelStrip.anchoredPosition = new Vector2(reelStrip.anchoredPosition.x, overshootY);

        // 
        float bounceElapsed = 0f;
        while (bounceElapsed < bounceDuration)
        {
            bounceElapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(bounceElapsed / bounceDuration);
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f); // Cubic ease-out

            float currentY = Mathf.Lerp(overshootY, finalY, easedProgress);
            reelStrip.anchoredPosition = new Vector2(reelStrip.anchoredPosition.x, currentY);
            yield return null;
        }

        reelStrip.anchoredPosition = new Vector2(reelStrip.anchoredPosition.x, finalY);
        IsSpinning = false;
        _spinRoutine = null;
    }


    private float GetYForSymbol(int index)
    {
        if (index < 0 || index >= reelStrip.childCount)
            return 0f;

        RectTransform symbol =
            reelStrip.GetChild(index) as RectTransform;

        // 
        return -symbol.localPosition.y + verticalOffset;
    }

}