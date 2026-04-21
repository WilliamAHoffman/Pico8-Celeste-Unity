using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    [Header("Defaults")]
    public float defaultDuration = 0.2f;
    public float defaultMagnitude = 0.15f;
    public AnimationCurve falloff = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private Vector3 originalLocalPos;
    private Coroutine shakeRoutine;

    void Awake()
    {
        originalLocalPos = transform.localPosition;
    }

    public void Shake()
    {
        Shake(defaultDuration, defaultMagnitude);
    }

    public void Shake(float duration, float magnitude)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        originalLocalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float strength = falloff.Evaluate(t) * magnitude;

            Vector2 offset = Random.insideUnitCircle * strength;
            transform.localPosition = originalLocalPos + new Vector3(offset.x, offset.y, 0f);

            yield return null;
        }

        transform.localPosition = originalLocalPos;
        shakeRoutine = null;
    }
}