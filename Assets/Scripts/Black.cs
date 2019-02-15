using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Black : MonoBehaviour
{
    public static Black Instance;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        Instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    public void FadeToClear(float duration)
    {
        StartCoroutine(FadeToClearCoroutine(duration));
    }

    public void FadeToBlack(float duration)
    {
        StartCoroutine(FadeToBlackCoroutine(duration));
    }

    private IEnumerator FadeToClearCoroutine(float duration)
    {
        float currTime = 0;
        Color color = spriteRenderer.color;

        while (currTime < duration)
        {
            currTime += Time.deltaTime;
            color.a = 1 - currTime / duration;
            spriteRenderer.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeToBlackCoroutine(float duration)
    {
        float currTime = 0;
        Color color = spriteRenderer.color;

        while (currTime < duration)
        {
            currTime += Time.deltaTime;
            color.a = currTime / duration;
            spriteRenderer.color = color;
            yield return null;
        }
        color.a = 1;
        spriteRenderer.color = color;
    }
}
