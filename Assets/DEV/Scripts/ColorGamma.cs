using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGamma : MonoBehaviour
{
    private Color originalColor;
    private Color gammaColor;

    void Start()
    {
        originalColor = new Color(0.6f, 0.7f, 0.8f);
        gammaColor = LinearToGammaSpace(originalColor);
    }

    public static Color LinearToGammaSpace(Color linearColor)
    {
        Color gammaColor = new Color();
        gammaColor.r = Mathf.RoundToInt(linearColor.r * 255) / 255f;
        gammaColor.g = Mathf.RoundToInt(linearColor.g * 255) / 255f;
        gammaColor.b = Mathf.RoundToInt(linearColor.b * 255) / 255f;
        gammaColor.a = linearColor.a;

        return gammaColor;
    }
}
