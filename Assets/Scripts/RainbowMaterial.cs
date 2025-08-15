using UnityEngine;
using System.Collections;

public class RainbowMaterial : MonoBehaviour
{
    [SerializeField] private Color currentColor;
    [SerializeField] private Color targetColor;
    [SerializeField] private float hue = 0f;
    
    
    
    public Renderer targetRenderer;
    public float transitionSpeed = 2f;

    private void Start()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        currentColor = targetRenderer.material.color;
        StartCoroutine(ColorTransition());

    }

    IEnumerator ColorTransition()
    {
       while (true)
        {
            hue += Time.deltaTime * 0.1f;
            if (hue > 1f) hue -= 1f;

            targetColor = Color.HSVToRGB(hue, 1f, 1f);
            currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * transitionSpeed);

            targetRenderer.material.color = currentColor;

            yield return null;
        }
    }
}
