using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyMovement : MonoBehaviour
{
    private RectTransform rectTransform;
    private System.Action onComplete;
    
    public void SetTarget(Vector2 spawnPosition, Transform targetTransform, System.Action onCompleteCallback)
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = spawnPosition;

        RectTransform parentRect = rectTransform.parent.GetComponent<RectTransform>();
        RectTransform targetRect = targetTransform.GetComponent<RectTransform>();

        Vector2 targetCenter = parentRect.InverseTransformPoint(targetRect.position);
        float halfWidth = targetRect.rect.width / 2f;

        float randomX = targetCenter.x + UnityEngine.Random.Range(-halfWidth, halfWidth);
        float randomY = targetCenter.y + UnityEngine.Random.Range(-20f, 20f);
        Vector2 targetPoint = new Vector2(randomX, randomY);

        float screenBottom = -parentRect.rect.height / 2f;
        Vector2 fallTarget = new Vector2(randomX, screenBottom - 100);

        float rotationSpeed = UnityEngine.Random.Range(100f, 300f);
        float initialRotation = UnityEngine.Random.Range(-30f, 30f);
        rectTransform.rotation = Quaternion.Euler(0, 0, initialRotation);

        onComplete = onCompleteCallback;

        StartCoroutine(MoveMoney(targetPoint, fallTarget, rotationSpeed));
    }

    private IEnumerator MoveMoney(Vector2 targetPoint, Vector2 fallTarget, float rotationSpeed)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;
        
        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPoint, elapsed / duration);
            rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        rectTransform.anchoredPosition = targetPoint;
        elapsed = 0f;
        duration = 1.2f;

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(targetPoint, fallTarget, elapsed / duration);
            rectTransform.Rotate(0, 0, rotationSpeed * 1.5f * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = fallTarget;
        onComplete?.Invoke();
        gameObject.SetActive(false); 
    }
}