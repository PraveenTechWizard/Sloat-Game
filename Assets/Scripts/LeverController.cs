using System.Collections;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("Lever References")]
    [SerializeField] private RectTransform leverPivot; // Pivot transform that compresses downward

    [Header("Pull Motion (Plunger / Compress)")]
    [SerializeField] private float minScaleY = 0.45f; // Minimum vertical scale at bottom of pull
    [SerializeField] private float pullDownDuration = 0.10f; // Fast pull downward
    [SerializeField] private float holdBottomDuration = 0.06f; // Brief pause at bottom
    [SerializeField] private float returnUpDuration = 0.18f; // Smooth spring-back to rest

    private Vector3 _defaultScale;
    private Coroutine _activePullRoutine;

    private void Awake()
    {
        if (leverPivot == null)
        {
            leverPivot = GetComponent<RectTransform>();
        }

        if (leverPivot != null)
        {
            _defaultScale = leverPivot.localScale;
        }
    }

    public void PullLever()
    {
        if (leverPivot == null) return;

        if (_activePullRoutine != null)
        {
            StopCoroutine(_activePullRoutine);
        }

        _activePullRoutine = StartCoroutine(PullRoutine());
    }

    private IEnumerator PullRoutine()
    {
        Vector3 pulledScale = new Vector3(_defaultScale.x, _defaultScale.y * minScaleY, _defaultScale.z);

        // 
        float elapsed = 0f;
        while (elapsed < pullDownDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / pullDownDuration);
            float easedProgress = progress * progress;

            leverPivot.localScale = Vector3.Lerp(_defaultScale, pulledScale, easedProgress);
            yield return null;
        }
        leverPivot.localScale = pulledScale;

        // 
        if (holdBottomDuration > 0f)
        {
            yield return new WaitForSeconds(holdBottomDuration);
        }

        // 
        elapsed = 0f;
        while (elapsed < returnUpDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / returnUpDuration);
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);

            leverPivot.localScale = Vector3.Lerp(pulledScale, _defaultScale, easedProgress);
            yield return null;
        }

        leverPivot.localScale = _defaultScale;
        _activePullRoutine = null;
    }
}