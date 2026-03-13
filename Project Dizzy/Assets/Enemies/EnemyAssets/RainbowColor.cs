using UnityEngine;

public class RainbowColor : MonoBehaviour
{
    [Header("Rainbow Settings")]
    [SerializeField] private float colorSpeed = 0.5f; // how fast the colors cycle

    private SpriteRenderer spriteRenderer;
    private float hue;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // move through hue spectrum
        hue += Time.deltaTime * colorSpeed;

        // keep hue between 0 and 1
        if (hue > 1f)
            hue -= 1f;

        // convert HSV to RGB
        Color newColor = Color.HSVToRGB(hue, 1f, 1f);

        // apply color to sprite
        spriteRenderer.color = newColor;
    }
}
