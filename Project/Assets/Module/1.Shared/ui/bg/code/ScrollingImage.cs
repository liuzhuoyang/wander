using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingImage : MonoBehaviour
{
    [SerializeField] private RawImage img;
    [SerializeField] private float x, y;

    bool isOn;

    private void OnEnable()
    {
        isOn = true;
    }

    private void OnDisable()
    {
        isOn = false;
    }

    void Update()
    {
        if (!isOn) return;
        img.uvRect = new Rect(img.uvRect.position + new Vector2(x, y) * Time.deltaTime, img.uvRect.size);
    }
}