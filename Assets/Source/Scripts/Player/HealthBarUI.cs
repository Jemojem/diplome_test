using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image healthSlider;
    [SerializeField] private RectTransform barTransform;

    [Header("Settings")]
    [SerializeField] private float fadeDelay = 1.5f;
    [SerializeField] private float fadeSpeed = 2f;

    [Header("Hit Animation")]
    [SerializeField] private float punchScale = 0.2f;
    [SerializeField] private float punchDuration = 0.2f;

    private Coroutine fadeCoroutine;
    private Tween punchTween;
    private Vector3 originalScale;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        originalScale = barTransform.localScale;
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthSlider.fillAmount = currentHealth / maxHealth;
        
        ShowWithHit();
    }

    private void ShowWithHit()
    {
        canvasGroup.alpha = 1f;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        
        if (punchTween != null && punchTween.IsActive())
            punchTween.Kill();

        barTransform.localScale = originalScale;
        punchTween = barTransform.DOPunchScale(Vector3.one * punchScale, punchDuration, 1, 0.5f);
        fadeCoroutine = StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        yield return new WaitForSeconds(fadeDelay);

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}