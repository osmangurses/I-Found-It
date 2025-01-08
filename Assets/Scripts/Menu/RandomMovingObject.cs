using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomMovingObject : MonoBehaviour
{
    public RectTransform panelRect; // Panelin RectTransform'u
    private Vector2 direction;
    public float speed = 50f;
    private float rotationSpeed;

    private void Start()
    {
        // Rastgele bir hareket yönü belirle
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        
        // Rastgele bir dönme hızı belirle
        rotationSpeed = Random.Range(-100f, 100f); // -100 ile 100 derece arasında rastgele hız
    }

    private void Update()
    {
        // Objenin hareketini sağlar
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition += direction * speed * Time.deltaTime;

        // Objenin dönmesini sağlar
        rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Sınır kontrolü yapar ve yön değiştirir
        Vector2 pos = rectTransform.anchoredPosition;
        Vector2 minBounds = panelRect.rect.min;
        Vector2 maxBounds = panelRect.rect.max;

        if (pos.x < minBounds.x || pos.x > maxBounds.x)
        {
            direction.x = -direction.x;
            pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        }
        if (pos.y < minBounds.y || pos.y > maxBounds.y)
        {
            direction.y = -direction.y;
            pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        }

        rectTransform.anchoredPosition = pos;
    }
}
