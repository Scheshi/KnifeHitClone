using UnityEngine;
using UnityEngine.UI;

public static class TextAdjuster
{
    public static void Adjust(this Text text, float width, float height)
    {
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 27);
        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    public static void Adjust(this Text text, float width, float height, Transform parent)
    {
        text.transform.parent = parent;
        Adjust(text, width, height);
    }

    public static void Adjust(this Text text, float width, float height, Transform parent, Vector2 position)
    {
        Adjust(text, width, height, parent);
        text.rectTransform.localPosition = position;
    }

    public static void Adjust(this Text text, float width, float height, Transform parent, Vector2 anchorMin, Vector2 anchorMax)
    {
        text.rectTransform.anchorMin = anchorMin;
        text.rectTransform.anchorMax = anchorMax;
        Adjust(text, width, height, parent);
    }

    public static void Adjust(this Text text, float width, float height, Transform parent, Vector2 position, Vector2 anchorMin, Vector2 anchorMax)
    {
        text.rectTransform.anchorMin = anchorMin;
        text.rectTransform.anchorMax = anchorMax;
        Adjust(text, width, height, parent, position);
    }
}
