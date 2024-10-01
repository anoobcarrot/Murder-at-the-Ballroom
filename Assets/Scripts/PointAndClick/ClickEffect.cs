using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public float duration = 0.5f;
    public float startScale = 0.1f;
    public float endScale = 1f;
    public Color startColor = Color.white;
    public Color endColor = new Color(1, 1, 1, 0);

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Play()
    {
        StartCoroutine(AnimateEffect());
    }

    private IEnumerator AnimateEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * endScale, t);
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
