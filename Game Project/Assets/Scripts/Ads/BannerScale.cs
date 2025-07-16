using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerScale : MonoBehaviour
{
    public RectTransform uiRoot;
    public Canvas canvas;
    private float originalAnchoredY;

    void Awake()
    {
        OnBannerShown();
    }

    public void OnBannerShown()
    {
        float shift = GetBannerHeightInUIUnits();
        Debug.Log($"Shifting UI up by {shift} units");

        Vector2 pos = uiRoot.anchoredPosition;
        pos.y = originalAnchoredY + shift;
        uiRoot.anchoredPosition = pos;
    }

    float GetBannerHeightInUIUnits()
    {
        float dpi = Screen.dpi;
        if (dpi == 0) dpi = 160f; // fallback for editor

        float bannerHeightPx = 50f * dpi / 160f; // 50dp standard height
        float scaleFactor = canvas != null ? canvas.scaleFactor : 1f;
        return bannerHeightPx / scaleFactor;
    }
}
