using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendedColor : MonoBehaviour
{

   [SerializeField] private GameObject[] targetObjects;
    [SerializeField] private List<Color> selectedColors = new List<Color>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Color"))
                {
                    Color clickedColor = hit.collider.GetComponent<Renderer>().material.color;
                    Debug.Log($"R={clickedColor.r}, G={clickedColor.g}, B={clickedColor.b}");

                    AddSelectedColor(clickedColor, hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Blend"))
                {
                    Color mixedColor = MixColors(selectedColors);

                    GameObject targetObj = FindTargetObject(hit.collider.gameObject);
                    if (targetObj != null)
                    {
                        targetObj.GetComponent<Renderer>().material.color = mixedColor;
                        targetObj.tag = "Color";
                    }

                    ClearSelectedColors();
                }
            }
        }
    }

    void ClearSelectedColors()
    {
        selectedColors.Clear();
    }

    void AddSelectedColor(Color color, GameObject clickedObject)
    {
        if (color != Color.white)
        {
            if (IsObjectEmpty(clickedObject))
                return;

            // Convert from gamma to linear space before adding to the list
            color = new Color(Mathf.GammaToLinearSpace(color.r), Mathf.GammaToLinearSpace(color.g), Mathf.GammaToLinearSpace(color.b));
            selectedColors.Add(color);
        }
    }

    bool IsObjectEmpty(GameObject obj)
    {
        return obj.CompareTag("Blend");
    }

    Color MixColors(List<Color> colors)
    {
        float totalRed = 0f, totalGreen = 0f, totalBlue = 0f;
        int count = 0;

        foreach (Color color in colors)
        {
            // Convert from gamma to linear space
            float linearRed = Mathf.GammaToLinearSpace(color.r);
            float linearGreen = Mathf.GammaToLinearSpace(color.g);
            float linearBlue = Mathf.GammaToLinearSpace(color.b);

            totalRed += linearRed;
            totalGreen += linearGreen;
            totalBlue += linearBlue;
            count++;
        }

        if (count == 0)
            return Color.white;

        float avgRed = totalRed / count;
        float avgGreen = totalGreen / count;
        float avgBlue = totalBlue / count;

        avgRed = Mathf.Clamp01(avgRed);
        avgGreen = Mathf.Clamp01(avgGreen);
        avgBlue = Mathf.Clamp01(avgBlue);

       
        avgRed = Mathf.LinearToGammaSpace(avgRed);
        avgGreen = Mathf.LinearToGammaSpace(avgGreen);
        avgBlue = Mathf.LinearToGammaSpace(avgBlue);

        return new Color(avgRed, avgGreen, avgBlue);
    }

    GameObject FindTargetObject(GameObject clickedSphere)
    {
        foreach (GameObject targetObj in targetObjects)
        {
            if (targetObj.GetComponent<Renderer>().bounds.Contains(clickedSphere.transform.position))
                return targetObj;
        }
        return null;
    }

}



