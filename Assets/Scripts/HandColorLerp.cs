using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColorLerp : MonoBehaviour
{
    public Renderer handRenderer;
    //public Color targetColor = Color.red;
    public float transitionDuration = 6f;
    public float resetDuration = 0.5f; // fast fade back



    private static readonly int ColorOverrideID = Shader.PropertyToID("_ColorOverride");
    private Coroutine colorChangeRoutine;

    void Start()
    {
        if (!handRenderer)
            handRenderer = GetComponentInChildren<Renderer>();
    }

    public void FadeToColor(Color newColor)
    {
        if (colorChangeRoutine != null)
            StopCoroutine(colorChangeRoutine);
        float duration = (newColor.a == 0f) ? resetDuration : transitionDuration;
        colorChangeRoutine = StartCoroutine(LerpColor(newColor, duration));
    }

    public void ResetColor()
    {
        FadeToColor(new Color(0f, 0f, 0f, 0f)); // RGB doesn't matter, alpha = 0 disables override
    }

    private IEnumerator LerpColor(Color newColor, float duration)
    {
        Color startColor = handRenderer.material.GetColor(ColorOverrideID);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Color current = Color.Lerp(startColor, newColor, elapsed / duration);
            handRenderer.material.SetColor(ColorOverrideID, current);
            yield return null;
        }

        handRenderer.material.SetColor(ColorOverrideID, newColor);
    }
}

