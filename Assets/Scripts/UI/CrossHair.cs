using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    RectTransform rect;

    [SerializeField] float maxSize = 200;
    [SerializeField] float minSize = 50;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    public void UpdateCrossHair(float normalizedAccuracy)
    {
        float newSize = minSize + (maxSize - minSize) * normalizedAccuracy;

        rect.sizeDelta = new Vector2 (newSize, newSize);
    }
}
